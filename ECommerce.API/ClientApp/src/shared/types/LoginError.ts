export interface LoginError {
    field?: 'email' | 'password' | 'general';
    message: string;
};
