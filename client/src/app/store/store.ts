import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";
import { errorApi } from "../../features/about/errorApi";
import { catalogApi } from "../../features/catalog/catalogApi";
import { counterSlice } from "../../features/contact/counterReducer";
import { uiSlice } from "../layout/uiSlice";

export const store = configureStore({
  reducer: {
    [catalogApi.reducerPath]: catalogApi.reducer,
    [errorApi.reducerPath]: errorApi.reducer,
    counter: counterSlice.reducer,
    ui: uiSlice.reducer,
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(catalogApi.middleware, errorApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();
