import { createApi } from "@reduxjs/toolkit/query/react";
import type { Product } from "../../app/models/product";
import { baseQueryWithEnhancements } from "../../app/api/baseApi";

export const catalogApi = createApi({
  reducerPath: "catalogApi",
  baseQuery: baseQueryWithEnhancements,
  // tagTypes: ["Product"],
  endpoints: (builder) => ({
    fetchProducts: builder.query<Product[], void>({
      query: () => "products",
      // providesTags: ["Product"],
    }),
    fetchProductDetails: builder.query<Product, number>({
      query: (productId) => `products/${productId}`,
      // providesTags: (_result, _error, id) => [{ type: "Product", id }],
    }),
  }),
});

// Export generated hooks
export const { useFetchProductsQuery, useFetchProductDetailsQuery } = catalogApi;
