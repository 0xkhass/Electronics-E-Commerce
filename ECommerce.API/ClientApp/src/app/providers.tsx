import { AuthProvider } from "../presentation/contexts/AuthContext";
import type { ReactNode } from "react";
export const AppProviders = ({children}: {children: ReactNode}) => 
{
    return <AuthProvider>{children}</AuthProvider>
};