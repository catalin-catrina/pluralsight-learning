import { Component, inject } from '@angular/core';
import { NgIf, NgFor, NgClass, AsyncPipe } from '@angular/common';
import { catchError, EMPTY } from 'rxjs';

import { ProductDetailComponent } from '../product-detail/product-detail.component';
import { ProductService } from '../product.service';

@Component({
  selector: 'pm-product-list',
  templateUrl: './product-list.component.html',
  standalone: true,
  imports: [AsyncPipe, NgIf, NgFor, NgClass, ProductDetailComponent],
})
export class ProductListComponent {
  // Just enough here for the template to compile
  pageTitle = 'Products';
  errorMessage = '';

  private productService = inject(ProductService);

  // Products
  readonly products$ = this.productService.products$.pipe(
    catchError((err) => {
      this.errorMessage = err;
      return EMPTY;
    })
  );

  // Selected product id to highlight the entry
  readonly selectedProductId$ = this.productService.productSelected$;

  onSelected(productId: number): void {
    this.productService.productSelected(productId);
  }
}
