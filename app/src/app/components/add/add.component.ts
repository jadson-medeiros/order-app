import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Order, Product } from '../../models/order.model';
import { OrderService } from '../../order.service';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css'],
})
export class AddComponent implements OnInit {
  orderForm!: FormGroup;
  products: Product[] = [
    { id: 1, productName: 'Smartphone XYZ', price: 1200.0 },
    { id: 2, productName: 'Notebook ABC', price: 2500.0 },
    { id: 3, productName: 'Bluetooth Headphones', price: 150.0 },
    { id: 4, productName: 'RGB Mechanical Keyboard', price: 180.0 },
    { id: 5, productName: 'Gamer Mouse XYZ', price: 80.0 },
  ];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.orderForm = this.formBuilder.group({
      customerName: ['', Validators.required],
      customerEmail: ['', [Validators.required, Validators.email]],
      isPaid: [false],
      selectedProducts: [[], Validators.required],
      quantity: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.orderForm.valid) {
      const order = this.createOrderFromForm();
      this.orderService.addOrder(order).subscribe(
        () => this.router.navigate(['/']),
        (error) => console.error('Error submitting order:', error)
      );
    } else {
      this.orderForm.markAllAsTouched();
    }
  }

  createOrderFromForm(): Order {
    const selectedProductIds = this.orderForm.value.selectedProducts;
    const selectedProducts = this.products.filter((product) =>
      selectedProductIds.includes(product.id)
    );

    return {
      id: 0,
      customerName: this.orderForm.value.customerName,
      customerEmail: this.orderForm.value.customerEmail,
      creationDate: new Date(),
      isPaid: this.orderForm.value.isPaid,
      orderItems: selectedProducts.map((product) => ({
        id: 0,
        productId: product.id,
        quantity: this.orderForm.value.quantity,
      })),
    };
  }

  goBackHome(): void {
    this.router.navigate(['/']);
  }
}
