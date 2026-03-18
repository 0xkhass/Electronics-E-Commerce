import { Navigate, Outlet } from "react-router-dom";
import { useAuthContext } from "../../presentation/contexts/AuthContext";

export const PublicRoute = () => {
    const { isAuthenticated } = useAuthContext();
    return isAuthenticated ? <Navigate to="/" replace /> : <Outlet />;
};