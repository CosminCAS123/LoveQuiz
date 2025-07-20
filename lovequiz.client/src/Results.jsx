import React, { useEffect, useState, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { FaCheckCircle, FaChevronLeft, FaChevronRight } from "react-icons/fa";
import TestimonialSlider from "./TestimonialSlider.jsx";
import './main-block.scss';
import './index.css';
import './tailwind.css';
import './header.scss';
import Header from "./Header.jsx";
import Footer from "./Footer.jsx";

export default function Results() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const isSmallScreen = window.innerWidth < 400;

  const { answers = {}, questions = [] } = state || {};

  const [report, setReport] = useState(null);
  const [error, setError] = useState(null);
  const [showIntro, setShowIntro] = useState(true);

  const [isAnimating, setIsAnimating] = useState(false);
  const [direction, setDirection] = useState("forward"); 

  const containerRef = useRef(null);
  const [lockedHeight, setLockedHeight] = useState(null);

  const benefits = [
    {
      title: "Claritate asupra a ceea ce nu functioneaza",
      description: "Intelege, in sfarsit, tiparele invizibile care creeaza distanta intre tine si partener.",
    },
    {
      title: "Afla daca TU contribui fara sa vrei la probleme",
      description: "Identifici comportamentele subtile care pot rani relatia, chiar daca intentiile tale sunt bune.",
    },
    {
      title: "Descopera unde increderea incepe sa se fisureze",
      description: "Obtii o imagine clara a nesigurantelor, indoielilor sau gesturilor care pot eroda increderea.",
    },
    {
      title: "Intelegi unde se pierd emotiile si afectiunea",
      description: "Afla de ce gesturile tale nu sunt intotdeauna primite asa cum ai intentionat.",
    },
    {
      title: "Comunicare mai buna — incepand de azi",
      description: "Iti aratam cum sa vorbesti si sa asculti astfel incat celalalt sa se simta cu adevarat inteles.",
    },
    {
      title: "Rezolva conflictele fara sa va raniti reciproc",
      description: "Primesti strategii reale pentru a iesi din certuri repetitive si a pastra legatura vie.",
    },
    {
      title: "Profil personalizat al relatiei tale",
      description: "Vezi cum functionezi tu in iubire si ce tensiuni apar in relatia voastra.",
    },
    {
      title: "Conexiune emotionala mai profunda",
      description: "Inveti cum sa creezi intimitate emotionala chiar si in momentele dificile.",
    },
    {
      title: "Reaprinde pasiunea si apropierea",
      description: "Identifici ce s-a pierdut intre voi si cum sa reconstruiti chimia pas cu pas.",
    },
    {
      title: "Potrivit pentru probleme reale de cuplu",
      description: "Fie ca sunteti impreuna de putin timp sau de ani de zile, raspunsurile se pliaza pe realitatea voastra.",
    },
    {
      title: "Functioneaza pentru orice etapa a relatiei",
      description: "Noua relatie, relatie veche sau in pragul despartirii — primesti exact ce ai nevoie.",
    },
    {
      title: "Rapid. Clar. Transformator.",
      description: "Nicio asteptare. Primesti in cateva minute raspunsuri care pot schimba totul.",
    },
  ];

  const itemsPerPage = 4;
  const [page, setPage] = useState(0);

  const totalPages = Math.ceil(benefits.length / itemsPerPage);
  const start = page * itemsPerPage;
  const currentItems = benefits.slice(start, start + itemsPerPage);

  const goNext = () => {
    if (page >= totalPages - 1) return;
    if (containerRef.current) {
      setLockedHeight(containerRef.current.offsetHeight);
    }
    setDirection("forward");
    setIsAnimating(true);
    setTimeout(() => {
      setPage((p) => p + 1);
      setIsAnimating(false);
      setLockedHeight(null);
    }, 400);
  };
  
  const goPrev = () => {
    if (page <= 0) return;
    if (containerRef.current) {
      setLockedHeight(containerRef.current.offsetHeight);
    }
    setDirection("back");
    setIsAnimating(true);
    setTimeout(() => {
      setPage((p) => p - 1);
      setIsAnimating(false);
      setLockedHeight(null);
    }, 400);
  };

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
      //add https://localhost:7279 before api for local development
    fetch("/api/quiz/free-report", {
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
          Șansele ca TU să fii problema
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
            x="55%"
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

      
        <div className="main-block-wrapper p-8 mx-0 space-y-8 text-center pb-12">
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
              {/* <div className="bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]"> */}
                <div className="bg-color-primary px-4 mb-6 pt-0 rounded-xl">
                  <h3 className="text-3xl font-bold mb-0">
                    <span dangerouslySetInnerHTML={{
                      __html: report.title.replace(/\*\*(.+?)\*\*/g, "<strong>$1</strong>"),
                    }} />
                  </h3>
                  
                  
                    <p className="text-md italic text-color-primary max-w-md mx-auto font-semibold">
                      {report.teaser}
                    </p>

                    {renderPieChart(report.severityLevel)}
                  

                </div>
              {/* </div> */}

              <button onClick={() => navigate("/results/paid")}
                className="quiz-button mt-4 mb-10">
                Acceseaza rezultatele
              </button>


              <div className="results_white_wrapper bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]">
                <div className="bg-color-primary p-6 rounded-2xl shadow-sm max-w-md mx-auto space-y-4">
                  <h3 className="text-3xl text-[#874B5C] font-black mb-8">
                    Esti la un pas de raspunsurile pe care nu ti le-a dat nimeni pana acum:
                  </h3>
                  
    
                  <div
                    ref={containerRef}
                    className="relative transition-all duration-300"
                    style={{
                      minHeight: isSmallScreen ? "500px" : "477px",
                    }}
                  >
                      {isAnimating ? (
                        <div className={`absolute inset-0 ${direction === "forward" ? "animate-fade-out-left" : "animate-fade-out-right"}`}>
                          {currentItems.map((benefit, idx) => (
                            <div key={idx} className="flex items-start space-x-3 pb-6">
                              <div className="pt-1">
                                <FaCheckCircle className="text-color-primary text-xl" />
                              </div>
                              <div>
                                <p className="font-semibold text-color-primary text-sm">{benefit.title}</p>
                                <p className="text-sm text-gray-500">{benefit.description}</p>
                              </div>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <div className="animate-grow ">
                          {currentItems.map((benefit, idx) => (
                            <div key={idx} className="flex items-start space-x-3 pb-6">
                              <div className="pt-1">
                                <FaCheckCircle className="text-color-primary text-xl" />
                              </div>
                              <div>
                                <p className="font-semibold text-color-primary text-sm">{benefit.title}</p>
                                <p className="text-sm text-gray-500">{benefit.description}</p>
                              </div>
                            </div>
                          ))}
                        </div>
                      )}
                    </div>


                  <div className="flex justify-center items-center space-x-4 pt-6 custom-button">
                    <button
                      onClick={goPrev}
                      disabled={page === 0}
                      className={`p-2 min-w-0 rounded-full transition ${
                        page === 0 ? "opacity-30 cursor-not-allowed" : "hover:bg-white/20"
                      }`}
                    >
                      <FaChevronLeft className="text-[#874B5C]" />
                    </button>

                    <span className="text-sm text-[#874B5C] font-medium">
                      Pagina {page + 1} / {totalPages}
                    </span>

                    <button
                      onClick={goNext}
                      disabled={page === totalPages - 1}
                      className={`transition min-w-0 ${
                        page === totalPages - 1 ? "opacity-30 cursor-not-allowed" : "hover:bg-white/20"
                      }`}
                    >
                      <FaChevronRight className="text-[#874B5C]" />
                    </button>
                  </div>
                </div>
              </div>

              <button onClick={() => navigate("/results/paid")}
                className="quiz-button my-8 button-margin">
                Afla cum sa faci asta
              </button>

              <div className="results_white_wrapper bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]">
                <div className="text-center space-y-3 shadow-sm bg-color-primary px-4 py-8 rounded-xl">
                  <h3 className="font-bold text-3xl mb-8">Vrei să aflii adevărul complet?</h3>
                  <p className="text-sm text-color-primary font-semibold">
                    Cine e mai posesiv? Cine domină emoțional?
                  </p>
                  <p className="text-sm text-color-primary mb-8 font-semibold">
                    Merită relația ta să continue așa?
                  </p>
                  
                  <div className="pointer-events-none mx-auto relative">

                    <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-24 h-24 text-gray-600">
                      <svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 448 512">
                        <path d="M400 224H384V144C384 64.47 319.5 0 240 0S96 64.47 96 144v80H80c-26.51 0-48 21.49-48 48v192c0 
                        26.5 21.49 48 48 48h320c26.51 0 48-21.5 48-48V272c0-26.5-21.5-48-48-48zM144 144c0-52.94 
                        43.06-96 96-96s96 43.06 96 96v80H144v-80zm112 208v40c0 8.84-7.2 16-16 16s-16-7.16-16-16v-40c-9.4-5.5-16-15.9-16-28 
                        0-17.7 14.3-32 32-32s32 14.3 32 32c0 12.1-6.6 22.5-16 28z"/>
                      </svg>
                    </div>

                    <div className="opacity-80 blur-[2px]">
                      <div className="bg-white/10 rounded p-4 mt-4 space-y-2 text-center text-sm">
                        <p className="font-semibold text-lg">Ce trebuie sa faci pentru a iti imbunatatii relația:</p>
                        <ul className="list-disc list-inside space-y-1 text-sm">
                          <li className="blur-[3px]">👀 Încredere: există suspiciuni sau temeri neexprimate.</li>
                          <li className="blur-[3px]">💬 Comunicare: poate fi dificilă sau tensionată uneori.</li>
                          <li className="blur-[4px]">❤️‍🔥 Atenție: dezechilibru în nevoia de afecțiune.</li>
                          <li className="blur-[4px]">🧠 Adaptabilitate: reacții diferite la schimbări sau conflicte.</li>
                          <li className="blur-[5px]">👀 Încredere: există suspiciuni sau temeri neexprimate.</li>
                          <li className="blur-[5px]">💬 Comunicare: poate fi dificilă sau tensionată uneori.</li>
                          <li className="blur-[5px]">❤️‍🔥 Atenție: dezechilibru în nevoia de afecțiune.</li>
                          <li className="blur-[5px]">🧠 Adaptabilitate: reacții diferite la schimbări sau conflicte.</li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <button onClick={() => navigate("/results/paid")}
                className="quiz-button my-8 button-margin">
                Vezi raportul complet
              </button>

              <div className="results_white_wrapper-testimonials bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]">
                <TestimonialSlider />
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

    <div className="footer-wrapper ">
      <Footer />
    </div>
    </>
  )
}