import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { Provider } from "react-redux";
import { RouterProvider } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "./app/layout/styles.css";
import { router } from "./app/routes/Routes.tsx";
import { store } from "./app/store/store.ts";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Provider store={store}>
      <ToastContainer position="bottom-right" theme="colored" />
      <RouterProvider router={router} />
    </Provider>
  </StrictMode>
);
