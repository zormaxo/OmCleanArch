import type { BaseQueryApi, FetchArgs } from "@reduxjs/toolkit/query";
import { fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { startLoading, stopLoading } from "../layout/uiSlice";

const customBaseQuery = fetchBaseQuery({
  baseUrl: "http://localhost:7700/api",
});

const sleep = () => new Promise((resolve) => setTimeout(resolve, 1000));

export const baseQueryWithErrorHandling = async (
  args: string | FetchArgs,
  api: BaseQueryApi,
  extraOptions: object
) => {
  api.dispatch(startLoading());
  await sleep();
  const result = await customBaseQuery(args, api, extraOptions);
  api.dispatch(stopLoading());
  if (result.error) {
    const { status, data } = result.error;
    // Optionally, handle error globally here
    console.log({ status, data });
  }
  return result;
};
