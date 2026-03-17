import axios from 'axios';

const BASE_URL = import.meta.env.VITE_API_URL;
export const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});
// Attach token to every request automatically
api.interceptors.request.use((config) => {
  const token = sessionStorage.getItem('TOKEN')

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Handle expired token globally
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      sessionStorage.removeItem('TOKEN')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)