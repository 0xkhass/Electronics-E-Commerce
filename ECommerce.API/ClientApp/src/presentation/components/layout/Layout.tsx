import { Outlet } from "react-router-dom";
import { Footer } from "./Footer";
import { Header } from "./Header";


export const Layout = () => {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1 pt-16">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
};