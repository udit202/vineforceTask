import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';

import { environment } from '../../../environments/environment';

interface Product {
  id: number;
  name?: string;
  description?: string;
  price?: number;
  sku?: string;
  stockQuantity?: number;
}

interface Payment {
  id?: number;
  orderId?: number;
  razorpayOrderId?: string;
  razorpayPaymentId?: string;
  paymentMethod?: string;
  status?: number | string;
  amount?: number;
  currency?: string;
}

interface Order {
  id: number;
  productId?: number;
  quantity?: number;

  customerName?: string;
  customerEmail?: string;
  customerPhone?: string;
  shippingAddress?: string;

  totalAmount?: number;
  amount?: number;

  status?: number | string;
  createdAt?: string;
  updatedAt?: string;

  product?: Product;
  payment?: Payment;
}

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css'
})
export class Orders implements OnInit {

  orders: Order[] = [];

  loading = false;
  errorMessage = '';

  selectedOrder: Order | null = null;

  private apiUrl = `${environment.apiBaseUrl}/OrderCrud`;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  // =========================
  // GET ALL ORDERS
  // =========================
  loadOrders(): void {
    this.loading = true;
    this.errorMessage = '';

    this.http.get<Order[]>(this.apiUrl).subscribe({
      next: (response) => {
        this.orders = response || [];
        this.loading = false;
      },

      error: (error) => {
        console.error('Error loading orders:', error);

        this.loading = false;

        this.errorMessage =
          error?.error?.message ||
          'Unable to load orders. Please try again.';
      }
    });
  }

  // =========================
  // GET ORDER BY ID
  // =========================
  viewOrder(orderId: number): void {

    this.http.get<Order>(`${this.apiUrl}/${orderId}`).subscribe({

      next: (order) => {
        this.selectedOrder = order;
      },

      error: (error) => {

        console.error('Error loading order:', error);

        Swal.fire({
          icon: 'error',
          title: 'Error',
          text:
            error?.error?.message ||
            'Unable to load order details.'
        });
      }

    });
  }

  // =========================
  // CLOSE ORDER DETAILS
  // =========================
  closeOrderDetails(): void {
    this.selectedOrder = null;
  }

  // =========================
  // UPDATE ORDER STATUS
  // =========================
  updateStatus(order: Order, status: number): void {

    const statusName = this.getOrderStatus(status);

    Swal.fire({
      title: 'Update Order Status?',
      text: `Change order #${order.id} status to ${statusName}?`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Yes, update',
      cancelButtonText: 'Cancel'
    }).then((result) => {

      if (!result.isConfirmed) {
        return;
      }

      this.http.put<Order>(
        `${this.apiUrl}/${order.id}/status`,
        {
          status: status
        }
      ).subscribe({

        next: (updatedOrder) => {

          const index = this.orders.findIndex(
            x => x.id === order.id
          );

          if (index !== -1) {
            this.orders[index] = updatedOrder;
          }

          if (this.selectedOrder?.id === order.id) {
            this.selectedOrder = updatedOrder;
          }

          Swal.fire({
            icon: 'success',
            title: 'Updated',
            text: 'Order status updated successfully.',
            timer: 1500,
            showConfirmButton: false
          });

        },

        error: (error) => {

          console.error(
            'Error updating order status:',
            error
          );

          Swal.fire({
            icon: 'error',
            title: 'Update Failed',
            text:
              error?.error?.message ||
              'Unable to update order status.'
          });

        }

      });

    });
  }

  // =========================
  // DELETE ORDER
  // =========================
  deleteOrder(order: Order): void {

    Swal.fire({
      title: 'Delete Order?',
      text: `Are you sure you want to delete order #${order.id}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#dc2626',
      cancelButtonText: 'Cancel',
      confirmButtonText: 'Yes, delete'
    }).then((result) => {

      if (!result.isConfirmed) {
        return;
      }

      this.http.delete(
        `${this.apiUrl}/${order.id}`
      ).subscribe({

        next: () => {

          this.orders = this.orders.filter(
            x => x.id !== order.id
          );

          if (this.selectedOrder?.id === order.id) {
            this.selectedOrder = null;
          }

          Swal.fire({
            icon: 'success',
            title: 'Deleted',
            text: 'Order deleted successfully.',
            timer: 1500,
            showConfirmButton: false
          });

        },

        error: (error) => {

          console.error(
            'Error deleting order:',
            error
          );

          Swal.fire({
            icon: 'error',
            title: 'Delete Failed',
            text:
              error?.error?.message ||
              'Unable to delete order.'
          });

        }

      });

    });
  }

  // =========================
  // ORDER STATUS
  // =========================
  getOrderStatus(
    status: number | string | undefined
  ): string {

    if (typeof status === 'string') {
      return status;
    }

    switch (status) {

      case 0:
        return 'Pending';

      case 1:
        return 'Confirmed';

      case 2:
        return 'Processing';

      case 3:
        return 'Shipped';

      case 4:
        return 'Delivered';

      case 5:
        return 'Cancelled';

      case 6:
        return 'Paid';

      default:
        return 'Unknown';
    }
  }

  // =========================
  // STATUS CLASS
  // =========================
  getStatusClass(
    status: number | string | undefined
  ): string {

    const statusName = this
      .getOrderStatus(status)
      .toLowerCase();

    switch (statusName) {

      case 'pending':
        return 'status-pending';

      case 'confirmed':
        return 'status-confirmed';

      case 'processing':
        return 'status-processing';

      case 'shipped':
        return 'status-shipped';

      case 'delivered':
        return 'status-delivered';

      case 'paid':
        return 'status-paid';

      case 'cancelled':
        return 'status-cancelled';

      default:
        return 'status-default';
    }
  }

  // =========================
  // PAYMENT STATUS
  // =========================
  getPaymentStatus(
    status: number | string | undefined
  ): string {

    if (typeof status === 'string') {
      return status;
    }

    switch (status) {

      case 0:
        return 'Created';

      case 1:
        return 'Authorized';

      case 2:
        return 'Captured';

      case 3:
        return 'Failed';

      case 4:
        return 'Refunded';

      default:
        return 'Unknown';
    }
  }

  // =========================
  // FORMAT DATE
  // =========================
  formatDate(
    date: string | undefined
  ): string {

    if (!date) {
      return '-';
    }

    return new Date(date).toLocaleString(
      'en-IN',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }
    );
  }

  // =========================
  // GET ORDER AMOUNT
  // =========================
  getOrderAmount(order: Order): number {

    if (order.totalAmount !== undefined) {
      return order.totalAmount;
    }

    if (order.amount !== undefined) {
      return order.amount;
    }

    if (order.payment?.amount !== undefined) {
      return order.payment.amount;
    }

    if (order.product?.price !== undefined) {
      return order.product.price *
        (order.quantity || 1);
    }

    return 0;
  }

  // =========================
  // GET TOTAL ORDER VALUE
  // =========================
  getTotalOrderValue(): number {

    return this.orders.reduce(
      (total, order) =>
        total + this.getOrderAmount(order),
      0
    );
  }

  // =========================
  // GET ORDERS BY STATUS
  // =========================
  getOrdersByStatus(status: string): number {

    return this.orders.filter(
      order =>
        this.getOrderStatus(order.status) === status
    ).length;
  }

  // =========================
  // TRACK BY
  // =========================
  trackByOrderId(
    index: number,
    order: Order
  ): number {

    return order.id;
  }
}
