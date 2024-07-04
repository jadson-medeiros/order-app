import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Order } from '../../models/order.model';
import { OrderService } from '../../order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css'],
})
export class OrderListComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'customerName',
    'customerEmail',
    'creationDate',
    'isPaid',
    'orderItems',
    'edit',
    'delete',
  ];
  dataSource: MatTableDataSource<Order> = new MatTableDataSource<Order>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private orderService: OrderService, private router: Router) {}

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe(
      (orders) => {
        this.dataSource.data = orders;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      (error) => {
        console.error('Error fetching orders:', error);
      }
    );
  }

  addOrder(): void {
    this.router.navigate(['/add']);
  }

  editOrder(order: Order): void {
    this.router.navigate(['/edit', order.id]);
  }

  deleteOrder(order: Order): void {
    if (confirm(`Are you sure you want to delete order ${order.id}?`)) {
      this.orderService.deleteOrder(order.id).subscribe(
        () => {
          this.fetchOrders();
        },
        (error) => {
          console.error('Error deleting order:', error);
        }
      );
    }
  }
}
