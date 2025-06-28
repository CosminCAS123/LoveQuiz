import React from 'react';
import './App.css';

function App() {
    const handleClick = () => {
        alert('Salut! Ai apasat butonul 🚀');
    };

    return (
        <div className="container" style={{ padding: '2rem', textAlign: 'center' }}>
            <button onClick={handleClick}>apasa-ma</button>
        </div>
    );
}

export default App;