import { useState } from "react";
import {  authService } from "../../infrastructure/services/authServices";
import type { AuthError } from "../../shared/types/auth/AuthError";
import type { LoginCredentials } from "../../shared/types/auth/AuthCredentials";
import { useAuthContext } from "../../presentation/contexts/AuthContext";
import type { UseAuthReturn } from "../../shared/types/auth/useAuthReturn";

export function useLogin(): UseAuthReturn {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<AuthError | null>(null);

  const { login } = useAuthContext();

  const clearError = () => setError(null);

  const handleLogin = async ({ email, password }: LoginCredentials) => {
    setIsLoading(true);
    setError(null);

    try {
      const data = await authService.loginApi(email, password);

      login(data);

      console.log("User Data: ", data);
      window.location.href = "/";
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      const status = error?.response?.status;

      console.log(status);

      if (status === 400) {
        setError({ field: "general", message: "Invalid email or password." });
      } else if (status === 422) {
        setError({
          field: "general",
          message: error?.response?.data?.message ?? "Validation failed.",
        });
      } else {
        setError({
          field: "general",
          message: "Something went wrong. Please try again.",
        });
      }
    } finally {
      setIsLoading(false);
    }
  };

  return { handleLogin, isLoading, error, clearError };
}
