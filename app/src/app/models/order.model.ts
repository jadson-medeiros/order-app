export interface OrderItem {
  productId: number;
  quantity: number;
  id: number;
}

export interface Order {
  id: number;
  customerName: string;
  customerEmail: string;
  creationDate: Date;
  isPaid: boolean;
  orderItems?: OrderItem[];
}

export interface Product {
  id: number;
  productName: string;
  price: number;
}
