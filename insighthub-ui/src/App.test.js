import React, { useEffect, useState } from "react";

const ApiTest = () => {
    const [articles, setArticles] = useState([]);

    useEffect(() => {
        fetch("/api/Article/semantic?query=climate")
            .then((res) => res.json())
            .then((data) => setArticles(data))
            .catch((err) => console.error("API error:", err));
    }, []);

    return (
        <div>
            <h2>Search Results</h2>
            <ul>
                {articles.map((a, i) => (
                    <li key={i}>
                        <strong>{a.title}</strong> ({a.year}) – {a.publisher}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ApiTest;
