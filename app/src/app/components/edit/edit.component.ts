import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Order, Product } from '../../models/order.model';
import { OrderService } from '../../order.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent implements OnInit {
  orderForm!: FormGroup;
  products: Product[] = [
    { id: 1, productName: 'Smartphone XYZ', price: 1200.0 },
    { id: 2, productName: 'Notebook ABC', price: 2500.0 },
    { id: 3, productName: 'Bluetooth Headphones', price: 150.0 },
    { id: 4, productName: 'RGB Mechanical Keyboard', price: 180.0 },
    { id: 5, productName: 'Gamer Mouse XYZ', price: 80.0 },
  ];
  orderId: number = 0;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.orderId = +this.route.snapshot.paramMap.get('id')!;

    this.orderForm = this.formBuilder.group({
      customerName: ['', Validators.required],
      customerEmail: ['', [Validators.required, Validators.email]],
      isPaid: [false],
      selectedProducts: [[], Validators.required],
      quantity: ['', Validators.required],
    });

    this.loadOrderDetails();
  }

  loadOrderDetails(): void {
    this.orderService.getOrderById(this.orderId).subscribe(
      (order: Order) => {
        this.orderForm.patchValue({
          customerName: order.customerName,
          customerEmail: order.customerEmail,
          isPaid: order.isPaid,
          selectedProducts: order.orderItems?.map((item) => item.productId),
          quantity:
            order.orderItems && order.orderItems.length > 0
              ? order.orderItems[0].quantity
              : '',
        });
      },
      (error) => {
        console.error('Error loading order details:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.orderForm.valid) {
      const selectedProductIds = this.orderForm.value.selectedProducts;
      const selectedProducts = this.products.filter((product) =>
        selectedProductIds.includes(product.id)
      );

      const order: Order = {
        id: this.orderId,
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

      this.orderService.updateOrder(order).subscribe(
        () => {
          this.router.navigate(['/']);
        },
        (error) => {
          console.error('Error updating order:', error);
        }
      );
    } else {
      this.orderForm.markAllAsTouched();
    }
  }

  goBackHome(): void {
    this.router.navigate(['/']);
  }
}
