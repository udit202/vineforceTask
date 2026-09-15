
import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';

import Swal from 'sweetalert2';

declare var Razorpay: any;

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  sku: string;
  stockQuantity: number;
  productUrl: string;
  isActive: boolean;
  imageUrl: string;
  createdAt: string;
  updatedAt: string | null;
}

interface OrderResponse {
  id: number;
  totalAmount: number;
  status: number;
}

interface PaymentResponse {
  id: number;
  orderId: number;
  razorpayOrderId: string;
  amount: number;
  currency: string;
  status: number;
}

interface VerifyPaymentResponse {
  id: number;
  orderId: number;
  status: number;
}

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {

  products: Product[] = [];

  loading = true;
  errorMessage = '';

  // Product currently being purchased
  buyingProductId: number | null = null;

  private productsApiUrl =
    `${environment.apiBaseUrl}/Products`;

  private orderApiUrl =
    `${environment.apiBaseUrl}/OrderCrud`;

  private paymentApiUrl =
    `${environment.apiBaseUrl}/PaymentCrud`;

  constructor(
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getProducts();
  }

  // ==========================================
  // GET PRODUCTS
  // ==========================================

  getProducts(): void {

    this.loading = true;
    this.errorMessage = '';

    console.log(
      'Products API:',
      this.productsApiUrl
    );

    this.http
      .get<Product[]>(this.productsApiUrl)
      .subscribe({

        next: (response) => {

          console.log(
            'Products received:',
            response
          );

          this.products = response;

          this.loading = false;

          this.cdr.detectChanges();
        },

        error: (error) => {

          console.error(
            'Products API error:',
            error
          );

          this.loading = false;

          this.errorMessage =
            'Unable to load products. Please try again.';

          this.cdr.detectChanges();
        }

      });
  }

  // ==========================================
  // BUY NOW
  // ==========================================

  buyNow(product: Product): void {

    console.log('================================');
    console.log('🔥 BUY NOW CLICKED');
    console.log('Product:', product);
    console.log('================================');

    // Prevent double click
    if (this.buyingProductId !== null) {

      console.log(
        'Another purchase is already processing.'
      );

      return;
    }

    // Check stock
    if (product.stockQuantity <= 0) {

      Swal.fire(
        'Out of Stock',
        'This product is currently out of stock.',
        'warning'
      );

      return;
    }

    // Check Razorpay SDK
    if (typeof Razorpay === 'undefined') {

      console.error(
        '❌ Razorpay SDK is not loaded.'
      );

      Swal.fire(
        'Razorpay Error',
        'Razorpay SDK is not loaded. Please refresh the page.',
        'error'
      );

      return;
    }

    this.buyingProductId = product.id;

    this.cdr.detectChanges();

    // ==========================================
    // STEP 1: CREATE ORDER
    // ==========================================

    const orderRequest = {

      productId: product.id,

      quantity: 1

    };

    console.log(
      '➡️ Calling Order API:',
      this.orderApiUrl
    );

    console.log(
      'Order Request:',
      orderRequest
    );

    this.http
      .post<OrderResponse>(
        this.orderApiUrl,
        orderRequest
      )
      .subscribe({

        next: (order) => {

          console.log(
            '✅ Order created:',
            order
          );

          if (!order || !order.id) {

            console.error(
              '❌ Invalid order response:',
              order
            );

            this.resetBuyingState();

            Swal.fire(
              'Order Error',
              'Invalid response received from order API.',
              'error'
            );

            return;
          }

          // ==========================================
          // STEP 2: CREATE PAYMENT
          // ==========================================

          this.createPayment(
            order.id,
            product
          );
        },

        error: (error) => {

          console.error(
            '❌ Order creation failed:',
            error
          );

          this.resetBuyingState();

          Swal.fire(
            'Order Error',
            error?.error?.message ||
            error?.error?.title ||
            'Failed to create order.',
            'error'
          );
        }

      });
  }

  // ==========================================
  // CREATE PAYMENT
  // ==========================================

  private createPayment(
    orderId: number,
    product: Product
  ): void {

    const paymentRequest = {

      orderId: orderId,

      currency: 'INR'

    };

    console.log(
      '================================'
    );

    console.log(
      '➡️ Calling Payment API:',
      this.paymentApiUrl
    );

    console.log(
      'Payment Request:',
      paymentRequest
    );

    console.log(
      '================================'
    );

    this.http
      .post<PaymentResponse>(
        this.paymentApiUrl,
        paymentRequest
      )
      .subscribe({

        next: (payment) => {

          console.log(
            '✅ Payment created:',
            payment
          );

          // Validate Razorpay Order ID
          if (
            !payment ||
            !payment.razorpayOrderId
          ) {

            console.error(
              '❌ Razorpay Order ID missing:',
              payment
            );

            this.resetBuyingState();

            Swal.fire(
              'Payment Error',
              'Razorpay Order ID was not returned by the backend.',
              'error'
            );

            return;
          }

          // ==========================================
          // STEP 3: OPEN RAZORPAY
          // ==========================================

          this.openRazorpay(
            payment,
            orderId,
            product
          );
        },

        error: (error) => {

          console.error(
            '❌ Payment creation failed:',
            error
          );

          this.resetBuyingState();

          Swal.fire(
            'Payment Error',
            error?.error?.message ||
            error?.error?.title ||
            'Failed to create payment.',
            'error'
          );
        }

      });
  }

  // ==========================================
  // OPEN RAZORPAY CHECKOUT
  // ==========================================

  private openRazorpay(
    payment: PaymentResponse,
    orderId: number,
    product: Product
  ): void {

    console.log(
      '================================'
    );

    console.log(
      '🚀 OPENING RAZORPAY'
    );

    console.log(
      'Razorpay SDK:',
      typeof Razorpay
    );

    console.log(
      'Razorpay Key:',
      environment.razorpayKey
    );

    console.log(
      'Razorpay Order ID:',
      payment.razorpayOrderId
    );

    console.log(
      'Amount:',
      payment.amount
    );

    console.log(
      'Currency:',
      payment.currency
    );

    console.log(
      '================================'
    );

    if (typeof Razorpay === 'undefined') {

      this.resetBuyingState();

      Swal.fire(
        'Razorpay Error',
        'Razorpay SDK is not loaded.',
        'error'
      );

      return;
    }

    if (!environment.razorpayKey) {

      this.resetBuyingState();

      Swal.fire(
        'Razorpay Error',
        'Razorpay Key ID is missing in environment configuration.',
        'error'
      );

      return;
    }

    if (!payment.razorpayOrderId) {

      this.resetBuyingState();

      Swal.fire(
        'Razorpay Error',
        'Razorpay Order ID is missing.',
        'error'
      );

      return;
    }

    // Razorpay expects amount in paise
    const amountInPaise =
      Math.round(
        Number(payment.amount) * 100
      );

    const options = {

      key: environment.razorpayKey,

      amount: amountInPaise,

      currency:
        payment.currency || 'INR',

      order_id:
        payment.razorpayOrderId,

      name: 'Vineforce Store',

      description:
        product.name,

      image:
        product.imageUrl,

      // ==========================================
      // PAYMENT SUCCESS
      // ==========================================

      handler: (response: any) => {

        console.log(
          '================================'
        );

        console.log(
          '✅ RAZORPAY PAYMENT SUCCESS'
        );

        console.log(
          'Razorpay Response:',
          response
        );

        console.log(
          '================================'
        );

        this.verifyPayment(
          orderId,
          response,
          product
        );
      },

      // ==========================================
      // CUSTOMER DETAILS
      // ==========================================

      prefill: {

        name: 'Test User',

        email: 'test@example.com',

        contact: '9999999999'

      },

      notes: {

        productId:
          product.id.toString(),

        orderId:
          orderId.toString()

      },

      theme: {

        color: '#2563eb'

      },

      // ==========================================
      // MODAL CLOSE
      // ==========================================

      modal: {

        ondismiss: () => {

          console.log(
            'Razorpay modal closed.'
          );

          this.resetBuyingState();
        }

      }

    };

    console.log(
      'Razorpay Options:',
      options
    );

    try {

      const razorpay =
        new Razorpay(options);

      console.log(
        '✅ Razorpay instance created.'
      );

      // ==========================================
      // PAYMENT FAILED
      // ==========================================

      razorpay.on(
        'payment.failed',
        (response: any) => {

          console.error(
            '❌ Razorpay payment failed:',
            response
          );

          this.resetBuyingState();

          Swal.fire(
            'Payment Failed',
            response?.error?.description ||
            'Payment could not be completed.',
            'error'
          );
        }
      );

      // ==========================================
      // OPEN RAZORPAY MODAL
      // ==========================================

      console.log(
        '🚀 Calling razorpay.open()'
      );

      razorpay.open();

    } catch (error) {

      console.error(
        '❌ Razorpay initialization error:',
        error
      );

      this.resetBuyingState();

      Swal.fire(
        'Razorpay Error',
        'Unable to open Razorpay checkout. Check browser console.',
        'error'
      );
    }
  }

  // ==========================================
  // VERIFY PAYMENT
  // ==========================================

  private verifyPayment(
    orderId: number,
    response: any,
    product: Product
  ): void {

    const verifyRequest = {

      orderId:
        orderId,

      razorpayOrderId:
        response.razorpay_order_id,

      razorpayPaymentId:
        response.razorpay_payment_id,

      razorpaySignature:
        response.razorpay_signature,

      paymentMethod:
        'Razorpay'

    };

    console.log(
      '➡️ Verifying payment:',
      verifyRequest
    );

    this.http
      .post<VerifyPaymentResponse>(
        `${this.paymentApiUrl}/verify`,
        verifyRequest
      )
      .subscribe({

        next: (result) => {

          console.log(
            '✅ Payment verification successful:',
            result
          );

          this.resetBuyingState();

          Swal.fire({

            icon: 'success',

            title: 'Payment Successful!',

            text:
              `${product.name} has been ordered successfully.`,

            confirmButtonText: 'OK'

          }).then(() => {

            // Refresh products so stock is updated
            this.getProducts();

          });
        },

        error: (error) => {

          console.error(
            '❌ Payment verification failed:',
            error
          );

          this.resetBuyingState();

          Swal.fire(
            'Verification Failed',
            error?.error?.message ||
            'Payment was completed, but payment verification failed.',
            'error'
          );
        }

      });
  }

  // ==========================================
  // RESET STATE
  // ==========================================

  private resetBuyingState(): void {

    this.buyingProductId = null;

    this.cdr.detectChanges();
  }
}
