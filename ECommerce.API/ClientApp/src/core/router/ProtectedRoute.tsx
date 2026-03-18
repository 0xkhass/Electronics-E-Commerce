import { Navigate, Outlet } from "react-router-dom";
import { useAuthContext } from "../../presentation/contexts/AuthContext";

export const ProtectedRoute = () => {
    const { isAuthenticated } = useAuthContext();
    return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};