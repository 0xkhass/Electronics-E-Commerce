import type { User } from "../User";

export interface AuthContextType {
    user: User | null;
    isAuthenticated: boolean;
    login: (data: User) => void;
    logout: () => void;
};