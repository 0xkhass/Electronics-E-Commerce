import { AuthProvider } from "../presentation/contexts/AuthContext";

export const AppProviders = ({children}: {children: React.ReactNode}) => 
{
    return <AuthProvider>{children}</AuthProvider>
};