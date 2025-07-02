# RTK Query Base Query Architecture

This module defines two key base query functions for RTK Query:

## 1. `baseQuery` (from `fetchBaseQuery`)

- **Type:** `BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError>`
- **Purpose:** Raw fetch logic, no side effects (no logging, loading, or error handling)
- **Returns:** `Promise<QueryReturnValue<Result, Error, Meta>>`
- **Usage:** Not recommended to use directly in production for complex apps

## 2. `baseQueryWithEnhancements` (custom wrapper)

- **Type:** `BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError>`
- **Purpose:** Adds logging, loading state management, and global error handling
- **Returns:** `Promise<QueryReturnValue<Result, Error, Meta>>`
- **Usage:** Should be used as the `baseQuery` in your `createApi` definitions

## Type Explanation

- `BaseQueryFn<Args, Result, Error>` is a function type:
  ```ts
  (args: Args, api: BaseQueryApi, extraOptions: DefinitionExtraOptions) =>
    Promise<QueryReturnValue<Result, Error, Meta>>;
  ```
- In this code, `Args` is `string | FetchArgs`:
  - `string`: Simple endpoint path (e.g. `"products"`)
  - `FetchArgs`: Full HTTP request config (e.g. `{ url, method, body, headers }`)
- Both `baseQuery` and `baseQueryWithEnhancements` conform to this contract, so they are interchangeable for RTK Query.

## Example Usage

**Simple GET endpoint (string):**

```ts
builder.query<Product[], void>({
  query: () => "products",
});
```

**Advanced POST endpoint (FetchArgs):**

```ts
builder.mutation<Product, Product>({
  query: (product) => ({
    url: "products",
    method: "POST",
    body: product,
    headers: { Authorization: "Bearer xyz" },
  }),
});
```

---

**Best Practice:**

- Use `baseQueryWithEnhancements` for all APIs to ensure consistent logging, loading, and error handling.
- Only use `baseQuery` directly for trivial or test cases.

**Note:**

- `baseQueryWithEnhancements` is a wrapper function. It delegates the actual HTTP request to `baseQuery`, but adds cross-cutting concerns (logging, loading, error handling) around it.
