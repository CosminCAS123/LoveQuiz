import React, { useEffect, useState, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { FaCheckCircle, FaChevronLeft, FaChevronRight, FaTimesCircle } from "react-icons/fa";
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
    console.log(submissions);
    if (!submissions.length) return;
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

    // fetch("/api/quiz/full-report", {
    //   method: "POST",
    //   headers: { "Content-Type": "application/json" },
    //   body: JSON.stringify(submissions),
    // })
    //   .then((res) =>{
    //     if (!res.ok) throw new Error(`HTTP ${res.status}`);
    //     return res.json();
    //   })
    //   .then((data) => {
    //     console.log(data);
    //   })
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


  const full_report = {
    adviceList: [
      "Încearcă să discuți deschis despre neliniștile tale cu partenerul, pentru a crea un spațiu de înțelegere reciprocă.",
      "Practică tehnici de relaxare care să te ajute să faci față momentelor de anxietate.",
      "Fii mai blândă cu tine însăți și acceptă că este normal să ai emoții intense în relații.",
      "Încurajează-i pe cei din jur să îți ofere feedback și să împărtășească cum se simt, pentru a ajuta la consolidarea comunicării.",
      "Setează momente de calitate împreună cu partenerul tău, unde să vă conectați profund fără distrageri.",
      "Amintește-ți că fiecare relație are momente dificile și este important să le abordați împreună, cu răbdare."
    ],
    aspects: [
      {
        aspect: "Comunicatie",
        description: "Îți exprimi gândurile și sentimentele, chiar dacă uneori te simți atacată. E un pas important spre crearea unei legături mai profunde.",
        score: 6
      },
      {
        aspect: "Încredere",
        description: "Ai tendința de a analiza situațiile și de a simți neliniște în momentele de incertitudine. Cultivarea încrederii în partener te va ajuta să te relaxezi.",
        score: 4
      },
      {
        aspect: "Empatie",
        description: "Îți pasă de sentimentele celuilalt și încerci să înțelegi reacțiile lui. Această empatie poate crea o atmosferă mai caldă în relații.",
        score: 7
      },
      {
        aspect: "Autocontrol",
        description: "În momentele de tensiune, reușești să te abții de la reacții impulsive, semn că îți dorești să rezolvi conflictele cu calm.",
        score: 5
      },
      {
        aspect: "Asertivitate",
        description: "Deși îți exprimi dorințele, uneori te simți nesigură sau neînțeleasă. Cultivarea încrederii în sine te va ajuta să comunici mai eficient nevoile tale.",
        score: 3
      }
    ],
    summary: "Observ că îți pasă profund de relația ta și dorești să simți o conexiune autentică. Deși încerci să gestionezi nesiguranțele cu răbdare…",
    title: "Între neliniște și dorința de conexiune",
    toxicityLevel: 25
  };
  console.log(full_report);

  return(
    <>
    <div className="header-wrapper">
      <Header />
    </div>

    <div className="main-block-wrapper p-8 space-y-6">
      <div className="title">
        <h3>RELATIA TA ESTE DE TIPUL:</h3>
        <h1>ANXIOS-PREOCUPAT</h1>
        <h3>-esti o Furtuna Anxioasa-</h3>
      </div>

      <div className="title__description">
        <h3 className="text-xl">Ce inseamna asta?</h3>
        <span>Dorești cu disperare apropierea și confirmarea iubirii, dar trăiești cu o frică constantă de respingere. Orice întârziere în răspuns sau lipsă de afecțiune te face să te simți nesigur. Ai o sensibilitate crescută la semne de respingere și uneori poți părea sufocant sau copleșitor. Intențiile tale sunt sincere, dar intensitatea emoțională poate destabiliza relația.</span>
      </div>

      <div className="emotional_needs_container results_white_wrapper bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]">
        <div className="text-center space-y-3 shadow-sm bg-color-primary px-4 py-8 rounded-xl">
          <h3 className="font-semibold text-2xl mb-8">Ce functioneaza?</h3>

          <div className="flex flex-col">
            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaCheckCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Esti inteles</p>
                <p className="text-sm text-gray-500">simti ca partenerul te “vede” din punct de vedere emotional</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaCheckCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Te simti in siguranta</p>
                <p className="text-sm text-gray-500">partenerul tau iti ofera siguranta emotionala si fizica</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaCheckCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Esti valorizat</p>
                <p className="text-sm text-gray-500">eforturile tale conteaza si sunt recunoscute</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaCheckCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Responsivitate emotionala</p>
                <p className="text-sm text-gray-500">partenerul tau raspunde atunci cand ai nevoie de sustinere</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaCheckCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Incredere</p>
                <p className="text-sm text-gray-500">ai certitudinea ca partenerul tau iti vrea binele </p>
              </div>
            </div>
          </div>
                  
        </div>
      </div>

      <div className="emotional_needs_container mt-10 results_white_wrapper bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]">
        <div className="text-center space-y-3 shadow-sm bg-color-primary px-4 py-8 rounded-xl">
          <h3 className="font-semibold text-2xl mb-8">Ce NU functioneaza?</h3>

          <div className="flex flex-col">
            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaTimesCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Esti inteles</p>
                <p className="text-sm text-gray-500">simti ca partenerul te “vede” din punct de vedere emotional</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaTimesCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Te simti in siguranta</p>
                <p className="text-sm text-gray-500">partenerul tau iti ofera siguranta emotionala si fizica</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaTimesCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Esti valorizat</p>
                <p className="text-sm text-gray-500">eforturile tale conteaza si sunt recunoscute</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaTimesCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Responsivitate emotionala</p>
                <p className="text-sm text-gray-500">partenerul tau raspunde atunci cand ai nevoie de sustinere</p>
              </div>
            </div>

            <div className="flex items-start space-x-3 pb-6">
              <div className="pt-2">
                <FaTimesCircle className="text-color-primary text-xl" />
              </div>
              <div>
                <p className="font-semibold text-color-primary text-sm">Incredere</p>
                <p className="text-sm text-gray-500">ai certitudinea ca partenerul tau iti vrea binele </p>
              </div>
            </div>
          </div>  
        </div>
      </div>


      <div className="aspects_container mt-10 mb-10">
        <h3 className="text-xl">Aspecte ale relatiei tale:</h3>
        <span className="block pt-4"><strong>Empatie:</strong> Îți pasă de sentimentele celuilalt și încerci să înțelegi reacțiile lui. Această empatie poate crea o atmosferă mai caldă în relații <strong>- 7/10</strong></span>
        <span className="block pt-4"><strong>Comunicatie:</strong> Îți exprimi gândurile și sentimentele, chiar dacă uneori te simți atacată. E un pas important spre crearea unei legături mai profunde <strong>- 6/10</strong></span>
        <span className="block pt-4"><strong>Autocontrol:</strong> În momentele de tensiune, reușești să te abții de la reacții impulsive, semn că îți dorești să rezolvi conflictele cu calm <strong>- 9/10</strong></span>
        <span className="block pt-4"><strong>Asertivitate:</strong> Deși îți exprimi dorințele, uneori te simți nesigură sau neînțeleasă. Cultivarea încrederii în sine te va ajuta să comunici mai eficient <strong>- 3/10</strong></span>
      </div>
        {renderPieChart(2)}


      <div className="attachment_container mt-10">
        <h3>Nevoia ta de atasament este:</h3>
        <h1>ANXIOS-PREOCUPAT</h1>
        <h3>Cauti validare constanta si cea mai mare frica a ta este respingerea</h3>

        <div className="w-[250px] h-[250px] bg-white mx-auto mt-5 text-center justify-center">Imagine sugestiva</div>
      </div>

      <h3 className="mt-10">Obiceiurile nesanatoase din relatie:</h3>
      <div className="unhealthy-habits_container mt-5 results_white_wrapper bg-white p-1 rounded-xl shadow-[0_30px_60px_-10px_rgba(233,184,195,0.8)]">
        <div className="text-center space-y-3 shadow-sm bg-color-primary px-4 py-8 rounded-xl">
          
          <h3 className="">TOXICITATE</h3>

          <div className="flex flex-col text-left items-start px-6 mt-5">
            <p className="font-semibold text-color-primary text-sm pb-6">Nu mai critica: <span className="text-gray-500"> foloseste constructii precum “eu simt” pentru a iti exprima nevoile fara a invinui </span> </p>
            <p className="font-semibold text-color-primary text-sm pb-6">Nu mai dispretui: <span className="text-gray-500"> practica zilnic respectul si admiratia, observa lucrurile bune la partenerul tau  </span> </p>
            <p className="font-semibold text-color-primary text-sm pb-6">Nu mai fii defensiv: <span className="text-gray-500"> fa o pauza din vorbit, asculta ce are de zis partenerul si recunoaste macar o parte din adevar </span> </p>
            <p className="font-semibold text-color-primary text-sm pb-6">Nu lasa furia sa te controleze: <span className="text-gray-500"> ia o pauza pentru a te calma si revino la discutie linistit </span> </p>
          </div>

          <div className="custom-button w-full flex items-space-between items-center">
            <button className="mr-auto">
              <FaChevronLeft className="text-[#874B5C]" />
            </button>
            <span className="justify-center text-center align-center">Pagina 1/10</span>
            <button className="ml-auto">
              <FaChevronRight className="text-[#874B5C]" />
            </button>
          </div>
        </div>
      </div>

      <h3 className="mt-10">Nivelul toxicitatii tale:</h3>
      <div className="flex">
        <div className="w-[150px] h-[150px] bg-white"></div>
        {renderPieChart(2)}
      </div>
      
      <h3 className="mt-10">Sfaturi finale:</h3>
      <div className="block">
        <span className="pt-4 block text-sm">-Încearcă să discuți deschis despre neliniștile tale cu partenerul, pentru a crea un spațiu de înțelegere reciprocă.</span>
        <span className="pt-4 block text-sm">-Practică tehnici de relaxare care să te ajute să faci față momentelor de anxietate.</span>
        <span className="pt-4 block text-sm">-Fii mai blândă cu tine însăți și acceptă că este normal să ai emoții intense în relații.</span>
        <span className="pt-4 block text-sm">-Încurajează-i pe cei din jur să îți ofere feedback și să împărtășească cum se simt, pentru a ajuta la consolidarea comunicării.</span>
        <span className="pt-4 block text-sm">-Setează momente de calitate împreună cu partenerul tău, unde să vă conectați profund fără distrageri.</span>
        <span className="pt-4 block text-sm">-Amintește-ți că fiecare relație are momente dificile și este important să le abordați împreună, cu răbdare.</span>
      </div>

      <h3 className="text-xl text-center mt-10">Si nu uita: invata sa asculti mai mult si sa vorbesti mai putin!</h3>

    </div>

    <div className="footer-wrapper ">
      <Footer />
    </div>
    </>
  )
}