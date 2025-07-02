/**
 * baseQueryWithEnhancements is a wrapper around baseQuery (fetchBaseQuery).
 * It adds logging, loading state, and global error handling.
 * For details and examples, see README.baseApi.md.
 */

import { fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type {
  BaseQueryFn,
  FetchArgs,
  FetchBaseQueryError,
} from "@reduxjs/toolkit/query";
import { startLoading, stopLoading } from "../layout/uiSlice";

const API_BASE_URL = "http://localhost:7700/api";
const sleep = () => new Promise((resolve) => setTimeout(resolve, 1000));

const baseQuery = fetchBaseQuery({
  baseUrl: API_BASE_URL,
});

// Centralized logging utility
const logRequest = (args: string | FetchArgs) => {
  const url = typeof args === "string" ? args : args.url;
  const method = typeof args === "string" ? "GET" : args.method || "GET";
  console.log(`🚀 API Request: ${method} ${API_BASE_URL}/${url}`);
};

const logResponse = (
  args: string | FetchArgs,
  result: { data?: unknown; error?: FetchBaseQueryError }
) => {
  const url = typeof args === "string" ? args : args.url;
  if (result.error) {
    console.error(`❌ API Error: ${url}`, result.error);
  } else {
    console.log(`✅ API Success: ${url}`, result.data);
  }
};

// Enhanced base query with logging, loading states, and error handling
export const baseQueryWithEnhancements: BaseQueryFn<
  string | FetchArgs,
  unknown,
  FetchBaseQueryError
> = async (args, api, extraOptions) => {
  // Pre-request logging
  logRequest(args);

  // Start loading state
  api.dispatch(startLoading());
  await sleep();

  try {
    // Execute the request
    const result = await baseQuery(args, api, extraOptions);

    // Post-request logging
    logResponse(args, result);

    // Global error handling
    if (result.error) {
      handleGlobalError(result.error, args);
    }

    return result;
  } catch (error) {
    // Handle unexpected errors
    console.error("🔥 Unexpected API Error:", error);
    return {
      error: {
        status: "FETCH_ERROR",
        error: String(error),
      } as FetchBaseQueryError,
    };
  } finally {
    // Always stop loading
    api.dispatch(stopLoading());
  }
};

// Global error handler
const handleGlobalError = (
  error: FetchBaseQueryError,
  args: string | FetchArgs
) => {
  const url = typeof args === "string" ? args : args.url;

  if (error.status === 401) {
    console.warn("🔐 Unauthorized access detected");
    // Handle unauthorized - maybe redirect to login
  } else if (error.status === 403) {
    console.warn("🚫 Forbidden access detected");
    // Handle forbidden
  } else if (error.status === 404) {
    console.warn("🔍 Resource not found:", url);
  } else if (error.status === 500) {
    console.error("💥 Server error detected");
    // Maybe show toast notification
  }

  // Add more global error handling logic here
};
