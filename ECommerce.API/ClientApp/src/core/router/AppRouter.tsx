import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "../../presentation/pages/LoginPage";
import RegisterPage from "../../presentation/pages/RegisterPage";
import { Layout } from "../../presentation/components/layout/Layout";
import { ProtectedRoute } from "./ProtectedRoute";
import { PublicRoute } from "./PublicRoute";

// Pages (replace with your actual imports as you build them)
// import HomePage from "../presentation/pages/HomePage";
// import ProductsPage from "../presentation/pages/ProductsPage";
// import CartPage from "../presentation/pages/CartPage";
// import ProfilePage from "../presentation/pages/ProfilePage";
// import OrdersPage from "../presentation/pages/OrdersPage";

export const AppRouter = () => {
  return (
    <BrowserRouter>
      <Routes>
        
        <Route element={<PublicRoute />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>

        {/* ── Public pages (with Layout) ── */}
        <Route element={<Layout />}>
          <Route path="/" element={<div>HomePage</div>} />
          <Route path="/products" element={<div>ProductsPage</div>} />
          <Route path="/products/:id" element={<div>ProductDetailPage</div>} />
          <Route path="/deals" element={<div>DealsPage</div>} />
          <Route path="/about" element={<div>AboutPage</div>} />
          <Route path="/cart" element={<div>CartPage</div>} />

          {/* ── Protected pages (must be logged in) ── */}
          <Route element={<ProtectedRoute />}>
            <Route path="/profile" element={<div>ProfilePage</div>} />
            <Route path="/orders" element={<div>OrdersPage</div>} />
            <Route path="/checkout" element={<div>CheckoutPage</div>} />
          </Route>
        </Route>

        {/* ── Fallback ── */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
};
