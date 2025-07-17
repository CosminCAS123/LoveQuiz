import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import './main-block.scss';
import './index.css';
import './tailwind.css';
import Header from "./Header.jsx";
import Footer from "./Footer.jsx";

export default function Results() {
  const navigate = useNavigate();
  const { state } = useLocation();

  const { answers = {}, questions = [] } = state || {};

  const [report, setReport] = useState(null);
  const [error, setError] = useState(null);
  const [showIntro, setShowIntro] = useState(true);

  const buildSubmissions = () =>
    questions
      .map((q) => {
        const selectedIdx = Array.isArray(answers) ? answers[q.id] : answers[q.id];
        if (selectedIdx == null) return null;
        const ansObj = q.answers[selectedIdx];
        if (!ansObj) return null;
        return {
          questionId: q.id,
          answerId: ansObj.id,
        };
      })
      .filter(Boolean);

  useEffect(() => {
    const submissions = buildSubmissions();
    if (!submissions.length) return;

    fetch("http://localhost:5023/api/quiz/free-report", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(submissions),
    })
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json();
      })
      .then((data) => {
        setReport(data);
        setTimeout(() => setShowIntro(false), 2500);
      })
      .catch((err) => setError(err.message));
  }, [state]);

  const getPiePercent = (level) => {
    const ranges = [
      [11, 14],
      [36, 39],
      [61, 64],
      [86, 89],
    ];
    const [min, max] = ranges[level];
    return Math.floor(Math.random() * (max - min + 1)) + min;
  };

  const renderPieChart = (level) => {
    const percent = getPiePercent(level);
    const color = ["#22c55e", "#eab308", "#f97316", "#dc2626"][level];
  
    return (
      <div className="flex flex-col items-center justify-center space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-color-primary">
          Șanse ca TU să fii problema
        </h2>
        <svg width="160" height="160" viewBox="0 0 36 36" className="relative">
          {/* Background circle */}
          <circle
            cx="18"
            cy="18"
            r="15.9155"
            fill="none"
            stroke="#f2f2f2"
            strokeWidth="3.8"
          />
          {/* Filled arc */}
          <circle
            cx="18"
            cy="18"
            r="15.9155"
            fill="none"
            stroke={color}
            strokeWidth="3.8"
            strokeDasharray={`${percent}, 100`}
            strokeDashoffset="0"
            strokeLinecap="round"
            transform="rotate(-90 18 18)"
          />
          {/* Percentage text */}
          <text
            x="50%"
            y="50%"
            dominantBaseline="middle"
            textAnchor="middle"
            fill="#924058"
            fontSize="6"
            fontWeight="bold"
          >
            {percent}%
          </text>
        </svg>
      </div>
    );
  };

  return (
    <>
      <div className="header-wrapper">
        <Header />
      </div>

      <div className="main-block-wrapper p-8 mx-0 space-y-8 text-center">
        {error && <p className="text-red-500">Eroare: {error}</p>}

        {showIntro && !error && (
          <div className="flex items-center justify-center h-[50vh] main-block-wrapper">
            <p className="text-xl text-color-primary animate-pulse">
              Analizăm rezultatele tale…
            </p>
          </div>
        )}

        {!showIntro && report && (
          <>
            <h1 className="text-2xl font-bold">
              <span dangerouslySetInnerHTML={{
                __html: report.title.replace(/\*\*(.+?)\*\*/g, "<strong>$1</strong>"),
              }} />
            </h1>

            <p className="text-md italic text-color-primary max-w-md mx-auto">
              {report.teaser}
            </p>

            {renderPieChart(report.severityLevel)}

            <div className="blur-[2px] opacity-80 pointer-events-none mx-auto">
              <div className="bg-white/10 rounded p-4 mt-4 space-y-2 text-center text-sm">
                <p className="font-semibold text-lg">Afla cum îți poți salva relația:</p>
                <ul className="list-disc list-inside space-y-1 text-sm">
                  <li className="blur-[3px]">👀 Încredere: există suspiciuni sau temeri neexprimate.</li>
                  <li className="blur-[3px]">💬 Comunicare: poate fi dificilă sau tensionată uneori.</li>
                  <li className="blur-[3px]">❤️‍🔥 Atenție: dezechilibru în nevoia de afecțiune.</li>
                  <li className="blur-[3px]">🧠 Adaptabilitate: reacții diferite la schimbări sau conflicte.</li>
                </ul>
              </div>
            </div>

            <div className="mt-8 text-center space-y-3">
              <p className="font-semibold text-lg">Vrei să știi adevărul complet?</p>
              <p className="text-sm text-color-primary">
                Cine e mai posesiv? Cine domină emoțional?
              </p>
              <p className="text-sm text-color-primary">
                Merită relația ta să continue așa?
              </p>
              <button onClick={() => navigate("/results/paid")}
                      className="quiz-button mt-2">
                Vezi raportul complet
              </button>
            </div>
          </>
        )}

        {!report && !error && !showIntro && (
          <p className="text-color-primary">Se generează raportul…</p>
        )}
      </div>

      <div className="footer-wrapper">
        <Footer />
      </div>
    </>
  );
}

export function PaidResults() {

  const full_report = {
    adviceList: [
      "Îmbunătățiți comunicarea deschisă pentru a evita neînțelegerile.",
      "Încercați să petreceți mai mult timp de calitate împreună pentru a întări conexiunea.",
      "Discutați despre sentimentele de neliniște pentru a construi încredere.",
      "Stabiliți limite clare legate de interacțiunile cu alte persoane pentru a evita gelozia.",
      "Fiecare dintre voi ar trebui să se simtă confortabil să își exprime nevoile și dorințele."
    ],
    aspects: [
      {
        aspect: "Comunicare",
        score: 4,
        description: "Partenera își exprimă sentimentele, dar uneori se simte atacată sau neliniștită."
      },
      {
        aspect: "Încredere",
        score: 5,
        description: "Există dubii referitoare la fidelitate, dar partenera încearcă să nu fie excesiv de gelos."
      },
      {
        aspect: "Atenție",
        score: 3,
        description: "Partenera simte că nu primește aceeași atenție, ceea ce îi provoacă neliniște."
      },
      {
        aspect: "Adaptabilitate",
        score: 6,
        description: "În ciuda dificultăților, partenera încearcă să se adapteze la situații noi."
      },
      {
        aspect: "Empatie",
        score: 5,
        description: "Se observă o dorință de a înțelege reacțiile partenerului, chiar și în momente dificile."
      }
    ],
    compatibilityVerdict: "Moderată",
    summary:
      "Partenera manifestă o atitudine de analiză și îngrijorare în relație, dar încearcă să rămână deschisă și să se adapteze. Există un mix d…",
    title: "Analiza relației de cuplu",
    toxicityLevel: 30
  };
  console.log(full_report);

  return(
    <>
    <div className="header-wrapper">
      <Header />
    </div>

    <div className="main-block-wrapper p-8 space-y-6">

    </div>

    <div className="footer-wrapper">
      <Footer />
    </div>
    </>
  )
}