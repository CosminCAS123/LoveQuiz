import Header from "./Header";
import Footer from "./Footer";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { X } from "../node_modules/lucide-react/dist/lucide-react";

export default function PaymentChecker() {
    const [error, setError] = useState(null);
    const sessionId = localStorage.getItem("quiz.sessionId");
    const navigate = useNavigate();

    useEffect(() => {
        if (!sessionId) {
            setError("Missing session id. Deschide din nou rezultatele după plată.");
            return;
        }

        let cancelled = false;

        const poll = async () => {
            try {
                const res = await fetch(`/api/quiz/session/${sessionId}/status`, {
                    headers: { "Cache-Control": "no-cache" },
                });

                if (res.status === 400) { if (!cancelled) setError("Format ID sesiune invalid."); return; }
                if (res.status === 404) { if (!cancelled) setError("Sesiunea nu a fost găsită."); return; }
                if (!res.ok) { if (!cancelled) setError(`Eroare status (${res.status}).`); return; }

                const payload = await res.json(); // either true/false OR { converted: bool } if backend not updated
                const converted = typeof payload === "boolean" ? payload : !!payload?.converted;

                if (!cancelled && converted) {
                    navigate("/results/paid", { replace: true });
                }
                // else keep polling
            } catch (e) {
                if (!cancelled) setError("Eroare de rețea la verificarea plății.");
            }
        };

        // run immediately then every 1.5s
        poll();
        const id = setInterval(poll, 1500);

        return () => {
            cancelled = true;
            clearInterval(id);
        };
    }, [sessionId, navigate]);

    return (
        <>
            <div className="header-wrapper">
                <Header />
            </div>

            <div className="main-block-wrapper p-8 mx-0 pb-12 relative flex">
                {error ? (
                    <p className="text-red-600 m-auto">{error}</p>
                ) : (
                    <p className="text-lg text-color-primary m-auto animate-pulse">
                        Analizăm rezultatele tale…
                    </p>
                )}
            </div>

            <div className="footer-wrapper">
                <Footer />
            </div>
        </>
    );
}