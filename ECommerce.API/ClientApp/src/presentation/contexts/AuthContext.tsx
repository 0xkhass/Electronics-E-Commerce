import { createContext, useContext, useEffect, useState } from "react";
import type { AuthContextType } from "../../shared/types/auth/AuthContextType";
import type { User } from "../../shared/types/User";
import { getCurrentUser } from "../../infrastructure/services/authServices";

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isInitializing, setIsInitializing] = useState(true);

  // On page load, check if the backend recognizes our cookie
  useEffect(() => {
    const checkSession = async () => {
      try {
        // This endpoint should return the User object if the cookie is valid
        const currentUser = await getCurrentUser(); 
        setUser(currentUser);
      } catch {
        // No valid cookie, user remains null
        setUser(null);
      } finally {
        setIsInitializing(false);
      }
    };

    checkSession();
  }, []);

  const login = (userData: User) => {
    setUser(userData); // Tokens are handled by the browser now!
  };

  const logout = async () => {
    await logout(); // Call backend to clear cookies
    setUser(null);
  };

  if (isInitializing) {
    return <div>Loading session...</div>; // Prevent flickering while checking auth
  }

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: !!user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuthContext = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuthContext must be used within AuthProvider");
  return context;
};

