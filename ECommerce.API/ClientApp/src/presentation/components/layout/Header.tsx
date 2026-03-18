import { useState, useEffect, useRef } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../contexts/AuthContext";
import { ChipIcon } from "../../../assets/icons/ChipIcon";
import { navLinks } from "../../../core/constants/navLinks";

export const Header = () => {
  const { user, isAuthenticated, logout } = useAuthContext();
  const [scrolled, setScrolled] = useState(false);
  const [searchOpen, setSearchOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [menuOpen, setMenuOpen] = useState(false);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const searchRef = useRef<HTMLInputElement>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const onScroll = () => setScrolled(window.scrollY > 20);
    window.addEventListener("scroll", onScroll);
    return () => window.removeEventListener("scroll", onScroll);
  }, []);

  useEffect(() => {
    if (searchOpen) searchRef.current?.focus();
  }, [searchOpen]);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setMenuOpen(false);
    setSearchOpen(false);
    setDropdownOpen(false);
  }, [location.pathname]);

  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(e.target as Node)
      )
        setDropdownOpen(false);
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, []);

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    if (searchQuery.trim()) {
      navigate(`/products?search=${encodeURIComponent(searchQuery.trim())}`);
      setSearchOpen(false);
      setSearchQuery("");
    }
  };

  

  const isActive = (path: string) => location.pathname === path;

  return (
    <header
      className={`fixed top-0 left-0 right-0 z-50 transition-all duration-300 ${
        scrolled
          ? "bg-(--charcoal-900)/90 backdrop-blur-md shadow-[0_1px_0_rgba(90,120,130,0.2),0_4px_24px_rgba(0,0,0,0.3)]"
          : "bg-transparent"
      }`}
    >
      {/* Main bar */}
      <div className="max-w-8xl mx-auto px-6 md:px-8 h-17 flex items-center gap-6">
        {/* ── Logo ── */}
        <Link
          to="/"
          className="flex items-center gap-2 no-underline shrink-0 group"
        >
          <div className="relative z-10">
            {/* Logo */}
            <div className="flex items-center gap-4 mb-10">
              <div className="p-2 rounded-xl bg-(--tan-500) text-(--charcoal-900)">
                <ChipIcon />
              </div>
              <span className="text-xl font-bold tracking-tight text-(--text-primary)">
                NEXUS<span className="text-(--tan-500)">TECH</span>
              </span>
            </div>
          </div>
        </Link>

        {/* ── Desktop Nav ── */}
        <nav className="hidden md:flex items-center gap-0.5 flex-1">
          {navLinks.map((link) => (
            <Link
              key={link.to}
              to={link.to}
              className={`relative px-3.5 py-2 radius-sm text-sm font-medium tracking-wide no-underline transition-all duration-200
                ${
                  isActive(link.to)
                    ? "text-(--tan-400)"
                    : "text-(--khaki-400) hover:text-(--text-primary) hover:bg-white/5"
                }`}
            >
              {link.label}
              {isActive(link.to) && (
                <span className="absolute bottom-0.5 left-1/2 -translate-x-1/2 w-4 h-0.5 bg-(--tan-400) rounded-full" />
              )}
            </Link>
          ))}
        </nav>

        {/* ── Actions ── */}
        <div className="flex items-center gap-2 ml-auto">
          {/* Search */}
          <div className="flex items-center gap-2">
            <form
              onSubmit={handleSearch}
              className={`overflow-hidden transition-all duration-300 ease-in-out ${
                searchOpen
                  ? "w-45 md:w-55 opacity-100"
                  : "w-0 opacity-0"
              }`}
            >
              <input
                ref={searchRef}
                type="text"
                placeholder="Search products..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onBlur={() => !searchQuery && setSearchOpen(false)}
                className="w-full h-9.5 px-3 text-sm bg-(--bg-input) border border-(--border-default) radius-md) text-(--text-primary) outline-none transition-all duration-200 placeholder:text-(--text-muted) focus:border-(--tan-500) focus:shadow-[0_0_0_3px_rgba(172,142,105,0.15)]"
              />
            </form>
            <button
              onClick={() => setSearchOpen((v) => !v)}
              className="w-9.5 h-9.5 flex items-center justify-center bg-white/4 border border-(--border-subtle)  radius-md text-(--khaki-400) hover:text-(--text-primary) hover:bg-white/8 hover:border-(--border-default) transition-all duration-200 cursor-pointer"
              aria-label="Search"
            >
              <svg
                width="17"
                height="17"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <circle cx="11" cy="11" r="8" />
                <path d="m21 21-4.35-4.35" />
              </svg>
            </button>
          </div>

          {/* Cart */}
          <Link
            to="/cart"
            className="relative w-9.5 h-9.5 flex items-center justify-center bg-white/4 border border-(--border-subtle) radius-md text-(--khaki-400) hover:text-(--text-primary) hover:bg-white/8 hover:border-(--border-default) transition-all duration-200 no-underline"
            aria-label="Cart"
          >
            <svg
              width="17"
              height="17"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
            >
              <path d="M6 2 3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z" />
              <line x1="3" y1="6" x2="21" y2="6" />
              <path d="M16 10a4 4 0 0 1-8 0" />
            </svg>
            <span className="absolute -top-1.5 -right-1.5 w-4.25 h-4.25 bg-(--tan-500) text-(--charcoal-900) text-[0.6rem] font-bold rounded-full flex items-center justify-center font-mono">
              3
            </span>
          </Link>

          {/* ── Logged In ── */}
          {isAuthenticated && user ? (
            <div ref={dropdownRef} className="relative">
              <button
                onClick={() => setDropdownOpen((v) => !v)}
                className="flex items-center gap-2 px-3 py-2 bg-white/4 border border-(--border-subtle) radius-md text-(--text-primary) hover:bg-white/8 hover:border-(--border-default) transition-all duration-200 text-sm cursor-pointer"
              >
                <span className="w-6.5 h-6.5 bg-(--tan-500) text-(--charcoal-900) rounded-full flex items-center justify-center text-xs font-bold font-mono shrink-0">
                  {user.fullName?.charAt(0).toUpperCase() ?? "U"}
                </span>
                <span className="hidden sm:block max-w-27.5 truncate text-(--khaki-300) font-medium">
                  {user.fullName}
                </span>
                <svg
                  width="11"
                  height="11"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2.5"
                  className={`text-(--khaki-500) transition-transform duration-200 ${dropdownOpen ? "rotate-180" : ""}`}
                >
                  <path d="m6 9 6 6 6-6" />
                </svg>
              </button>

              {/* Dropdown */}
              <div
                className={`absolute top-[calc(100%+8px)] right-0 min-w-44 bg-(--charcoal-700) border border-(--border-default) radius-md shadow-(--shadow-modal) p-1.5 z-50 transition-all duration-200 ${
                  dropdownOpen
                    ? "opacity-100 translate-y-0 pointer-events-auto"
                    : "opacity-0 -translate-y-1.5 pointer-events-none"
                }`}
              >
                <Link
                  to="/profile"
                  className="block px-3 py-2 radius-sm text-sm text-(--khaki-300) no-underline font-normal hover:bg-white/7 hover:text-(--text-primary) transition-colors duration-150"
                >
                  Profile
                </Link>
                <Link
                  to="/orders"
                  className="block px-3 py-2 radius-sm text-sm text-(--khaki-300) no-underline font-normal hover:bg-white/7 hover:text-(--text-primary) transition-colors duration-150"
                >
                  My Orders
                </Link>
                <hr className="my-1 border-(--border-subtle)" />
                <button
                  onClick={logout}
                  className="w-full text-left px-3 py-2 radius-sm text-sm text-(--color-error) bg-transparent border-none font-normal hover:bg-[rgba(224,92,92,0.1)] transition-colors duration-150 cursor-pointer"
                >
                  Sign Out
                </button>
              </div>
            </div>
          ) : (
            /* ── Logged Out ── */
            <div className="hidden sm:flex items-center gap-2">
              <Link
                to="/login"
                className="px-3.5 py-2 text-sm font-medium (--khaki-300) no-underline (--radius-md hover:text-(--text-primary) hover:bg-white/6 transition-all duration-200"
              >
                Sign In
              </Link>
              <Link
                to="/register"
                className="px-4 py-2 text-sm font-semibold bg-(--tan-500) text-(--charcoal-900) no-underline radius-md hover:bg-(--tan-400) hover:shadow-[0_4px_16px_rgba(172,142,105,0.3)] transition-all duration-200"
              >
                Sign Up
              </Link>
            </div>
          )}

          {/* Hamburger */}
          <button
            className="md:hidden flex flex-col justify-center gap-1.25 w-9 h-9 bg-transparent border-none cursor-pointer p-1.5"
            onClick={() => setMenuOpen((v) => !v)}
            aria-label="Toggle menu"
          >
            <span
              className={`block w-full h-0.5 bg-(--khaki-400) rounded transition-all duration-250 origin-center ${menuOpen ? "translate-y-1.75 rotate-45" : ""}`}
            />
            <span
              className={`block w-full h-0.5 bg-(--khaki-400) rounded transition-all duration-250 ${menuOpen ? "opacity-0 scale-x-0" : ""}`}
            />
            <span
              className={`block w-full h-0.5 bg-(--khaki-400) rounded transition-all duration-250 origin-center ${menuOpen ? "-translate-y-1.75 -rotate-45" : ""}`}
            />
          </button>
        </div>
      </div>

      {/* ── Mobile Menu ── */}
      <div
        className={`md:hidden border-t border-(--border-subtle) bg-(--charcoal-900) overflow-hidden transition-all duration-300 ${
          menuOpen ? "max-h-100 opacity-100" : "max-h-0 opacity-0"
        }`}
      >
        <nav className="flex flex-col gap-1 p-4">
          {navLinks.map((link) => (
            <Link
              key={link.to}
              to={link.to}
              className={`px-4 py-3 radiux-md text-sm font-medium no-underline transition-all duration-200
                ${
                  isActive(link.to)
                    ? "text-(--tan-400)] bg-[rgba(172,142,105,0.08)]"
                    : "text-(--khaki-400)] hover:text-(--text-primary) hover:bg-white/5"
                }`}
            >
              {link.label}
            </Link>
          ))}

          {!isAuthenticated && (
            <div className="flex gap-2 pt-3 mt-2 border-t border-(--border-subtle)">
              <Link
                to="/login"
                className="flex-1 text-center px-4 py-2.5 text-sm font-medium text-(--khaki-300) no-underline border border-(--border-default) radius-md hover:bg-white/5 transition-all duration-200"
              >
                Sign In
              </Link>
              <Link
                to="/register"
                className="flex-1 text-center px-4 py-2.5 text-sm font-semibold bg-(--tan-500)] text-[var(--charcoal-900) no-underline radius-md hover:bg-(--tan-400) transition-all duration-200"
              >
                Sign Up
              </Link>
            </div>
          )}
        </nav>
      </div>
    </header>
  );
};
