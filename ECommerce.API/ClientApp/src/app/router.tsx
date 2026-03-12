import { BrowserRouter, Routes, Route } from "react-router-dom";
import LoginPage from "../presentation/pages/auth/LoginPage";


export const AppRouter = () => {
    return (
        <BrowserRouter>
            <Routes>
                {/*TODO: replace null elements with the real views */}
                <Route path="/login" element={<LoginPage />}/>
                <Route path="/register" element={null} />
                <Route path="/" element={null} />
            </Routes>
        </BrowserRouter>
    );
};