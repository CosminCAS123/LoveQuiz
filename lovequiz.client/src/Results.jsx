import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import './main-block.scss';
import './tailwind.css';
import Header from "./Header.jsx";
import Footer from "./Footer.jsx";

export default function Results() {
  const navigate = useNavigate();
  const { state } = useLocation();

  const { answers = {}, questions = [] } = state || {};

  const [report, setReport] = useState(null);
  const [error,  setError]  = useState(null);

  const full_report = {
    adviceList: [
      "Încurajează o comunicare deschisă și sinceră; exprimă-ți nevoile fără a blama.",
      "Stabiliți momente de conectare, fără distrageri, pentru a întări relația."
    ],
    compatibilityVerdict: "Există riscuri mari de conflict și neînțelegeri.",
    keyInsights: [
      "Partenerul tău poate manifesta comportamente de evitare atunci când vine vorba de discuții dificile.",
      "Sentimentele de nesiguranță și anxietate sunt prezente, iar comunicarea nu este eficientă."
    ],
    summary: "Relația prezintă semne de tensiune și neînțelegeri, cu o comunicare deficitară și un sentiment de nesiguranță din partea partenerului. Est…",
    title: "Analiza relației de cuplu",
    toxicityLevel: 7
  };
  console.log(full_report);


  const buildSubmissions = () =>
    questions                       // iterate over the questions we have
      .map((q) => {
        // works whether `answers` is array or object
        const selectedIdx = Array.isArray(answers)
          ? answers[q.id]
          : answers[q.id];
  
        if (selectedIdx == null) return null;
  
        // real DB id of the chosen answer:
        const ansObj = q.answers[selectedIdx];
        if (!ansObj) return null; 
  
        return {
          questionId: q.id,     // camelCase → matches QuizSubmissionDto
          answerId:   ansObj.id // NOT the array index
        };
      })
      .filter(Boolean);
  
  useEffect(() => {
    const submissions = buildSubmissions();
    console.log(JSON.stringify(submissions));
    if (!submissions.length) return; 

    fetch("http://localhost:5023/api/quiz/free-report", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(submissions),
    })
      .then(res => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json();
      })
      .then(data => setReport(data))
      .catch(err => setError(err.message));
    

    //FULL REPORT API CALL - EACH CALL COSTS A FEW FRACTIONS OF ONE LEU, REFER TO VARIABLE 'full_report' WHILE IMPLEMENTING FRONTEND
    // fetch("http://localhost:5023/api/quiz/full-report", {
    //   method: "POST",
    //   headers: { "Content-Type": "application/json" },
    //   body: JSON.stringify(submissions),
    // })
    //   .then((res) => {
    //     if (!res.ok) throw new Error(`HTTP ${res.status}`);
    //     return res.json();
    //   })
    //   .then((data) => {
    //     console.log("Full Report:", data);
    //   })
    //   .catch((err) => {
    //     console.error("Full Report Error:", err.message);
    //   });
    
  }, [state]); 

  

  if (!questions.length) {
    return (
      <div className="p-8">
        <p>Nu există rezultate. Încearcă să completezi chestionarul mai întâi!</p>
        <button onClick={() => navigate("/")} className="mt-4 quiz-button">
          Înapoi la început
        </button>
      </div>
    );
  }


  return (

    <>
    <div className="header-wrapper">
      <Header />
    </div>
    
    <div className="p-8 space-y-6 main-block-wrapper">
      <h1 className="text-2xl font-bold">Rezultatele tale</h1>

      {error && (
        <p className="text-red-500">Eroare la generarea raportului: {error}</p>
      )}
      {report && (
        <div className="mt-6 border rounded p-4 bg-white/5 space-y-2">
          <h2
            className="text-xl font-semibold"
            dangerouslySetInnerHTML={{
              __html: report.title.replace(/\*\*(.+?)\*\*/g, "<strong>$1</strong>"),
            }}
          />

          <p>{report.teaser}</p>

          {(() => {
            const labels = ["None", "Minor", "Moderat", "Sever"];
            const colors = [
              "text-green-500",
              "text-yellow-500",
              "text-orange-500",
              "text-red-600",
            ];
            const lvl = report.severityLevel ?? 0;
            return (
              <p className={`font-bold ${colors[lvl]}`}>
                Severitate: {labels[lvl]} (nivel {lvl})
              </p>
            );
          })()}
        </div>
      )}

      <button onClick={() => navigate("/")} className="mt-4 quiz-button">
        Înapoi la început
      </button>
    </div>

      <div className="footer-wrapper">
        <Footer />
      </div>
    </>
  );
}