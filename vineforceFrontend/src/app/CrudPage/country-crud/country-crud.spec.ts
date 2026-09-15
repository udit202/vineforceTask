import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CountryCrud } from './country-crud';

describe('CountryCrud', () => {
  let component: CountryCrud;
  let fixture: ComponentFixture<CountryCrud>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CountryCrud],
    }).compileComponents();

    fixture = TestBed.createComponent(CountryCrud);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
