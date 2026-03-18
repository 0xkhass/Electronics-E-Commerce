import { useState } from "react";
import type { AuthError } from "../../shared/types/auth/AuthError";
import type { RegisterCredentials } from "../../shared/types/auth/RegisterCredentials";
import { registerApi } from "../../infrastructure/services/authServices";
import type { UseAuthReturn } from "../../shared/types/auth/useAuthReturn";
import { useAuthContext } from "../../presentation/contexts/AuthContext";


export function useRegister(): UseAuthReturn {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<AuthError | null>(null);

  const { login } = useAuthContext();

  const clearError = () => setError(null);

  const handleRegister = async ({
    email,
    password,
    userName,
    firstName,
    lastName
  }: RegisterCredentials) => {
    setIsLoading(true);
    setError(null);

    try {
      const data = await registerApi(
        email,
        password,
        userName,
        firstName,
        lastName
      );

      login(data);

      console.log(data.user);
      window.location.href = "/";
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      const status = error?.response?.status;

      if (status === 400) {
        setError({
          field: "general",
          message: error?.response?.data?.message ?? "Registration failed.",
        });
      } else if (status === 409) {
        setError({
          field: "general",
          message: "Email or username already exists.",
        });
      } else if (status === 422) {
        setError({
          field: "general",
          message: error?.response?.data?.message ?? "Validation failed.",
        });
      } else if (!error.response) {
        setError({
          field: "general",
          message: "Network error. Please check your connection.",
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

  return { handleRegister, isLoading, error, clearError };
}
