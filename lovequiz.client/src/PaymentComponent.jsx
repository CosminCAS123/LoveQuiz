import Header from "./Header";
import Footer from "./Footer";
import { useState, useEffect } from "react";

export default function PaymentChecker() {

    const [phase, setPhase] = useState("polling");
    const [error, setError] = useState(null);
    const [report, setReport] = useState(null);

    const sessionId = localStorage.getItem("quiz.sessionId");
    console.log(sessionId);

    useEffect(() => {
        if (!sessionId) {
        setError("Missing session id.");
        setPhase("error");
        return;
        }

        let cancelled = false;
        const controller = new AbortController();

        async function tick() {
        try {
            const res = await fetch(`/api/quiz/session/${sessionId}/status`, {
            signal: controller.signal,
            });
            if (!res.ok) {
            const msg =
                res.status === 404
                ? "Session not found."
                : res.status === 400
                ? "Bad request."
                : `Status check failed (${res.status}).`;
            if (!cancelled) {
                setError(msg);
                setPhase("error");
            }
            return;
            }

            const data = await res.json(); // expect { converted: boolean }
            if (data?.converted === true) {
            if (!cancelled) {
                setPhase("generating");
                const fr = await fetch(`/api/quiz/full-report`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ sessionId }), // adjust if your API expects a different DTO
                });
                if (!fr.ok) {
                setError(`Full report failed (${fr.status}).`);
                setPhase("error");
                return;
                }
                const payload = await fr.json();
                setReport(payload);
                setPhase("ready");
            }
            }
        } catch (e) {
            if (!cancelled && e.name !== "AbortError") {
            setError("Network error while checking status.");
            setPhase("error");
            }
        }
        }

        // run immediately, then every 2.5s
        tick();
        const id = setInterval(tick, 1500);

        return () => {
        cancelled = true;
        controller.abort();
        clearInterval(id);
        };
    }, [sessionId]);

    
    return (
<>
    <div className="header-wrapper">
        <Header />
    </div>

    <div className="main-block-wrapper p-8 mx-0 space-y-8 text-center pb-12 relative flex">
        {phase === "polling" && (
          <p className="text-lg animate-pulse justify-self-center self-center m-auto animate-pulse">Plata se procesează...</p>
        )}
        {phase === "generating" && (
          <p className="text-lg justify-self-center self-center m-auto animate-pulse">Generăm raportul…</p>
        )}
        {phase === "ready" && (
          <pre className="text-left max-w-3xl overflow-auto p-4 bg-black/5 rounded-lg justify-self-center self-center m-auto">
            {JSON.stringify(report, null, 2)}
          </pre>
        )}
        {phase === "error" && (
          <p className="text-red-600 justify-self-center self-center m-auto">Eroare: {error}</p>
        )}
    </div>

    <div className="footer-wrapper" >
        <Footer />
    </div>
</>
    );
}