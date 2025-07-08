import { createApi } from "@reduxjs/toolkit/query/react";
import { baseQueryWithEnhancements } from "../../app/api/baseApi";

export const errorApi = createApi({
  reducerPath: "errorApi",
  baseQuery: baseQueryWithEnhancements,
  endpoints: (builder) => ({
    get400Error: builder.query<void, void>({
      query: () => ({ url: "buggycustomresult/bad-request-custom" }),
    }),
    get401Error: builder.query<void, void>({
      query: () => ({ url: "buggycustomresult/unauthorized-custom" }),
    }),
    get404Error: builder.query<void, void>({
      query: () => ({ url: "buggycustomresult/not-found-custom" }),
    }),
    get500Error: builder.query<void, void>({
      query: () => ({ url: "buggycustomresult/server-error-custom" }),
    }),
    getValidationError: builder.query<void, void>({
      query: () => ({ url: "buggycustomresult/validation-error-custom" }),
    }),
  }),
});

export const {
  useLazyGet400ErrorQuery,
  useLazyGet401ErrorQuery,
  useLazyGet500ErrorQuery,
  useLazyGet404ErrorQuery,
  useLazyGetValidationErrorQuery,
} = errorApi;
