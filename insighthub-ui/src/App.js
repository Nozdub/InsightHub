import React, { useState } from "react";
import "./App.css";

function App() {
    const [query, setQuery] = useState("");
    const [articles, setArticles] = useState([]);
    const [favorites, setFavorites] = useState([]);
    const [searchHistory, setSearchHistory] = useState([]);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: "asc" });
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 20;

    const handleSearch = () => {
        if (!query.trim()) return;

        setSearchHistory((prev) => [query, ...prev.filter((q) => q !== query)]);
        setCurrentPage(1); // reset to first page on new search

        fetch(`/api/Article/semantic?query=${encodeURIComponent(query)}`)
            .then((res) => res.json())
            .then((data) => setArticles(data))
            .catch((err) => console.error("Error fetching articles:", err));
    };

    const toggleFavorite = (title) => {
        setFavorites((prev) =>
            prev.includes(title) ? prev.filter((t) => t !== title) : [...prev, title]
        );
    };

    const handleSort = (key) => {
        let direction = "asc";
        if (sortConfig.key === key && sortConfig.direction === "asc") {
            direction = "desc";
        }
        setSortConfig({ key, direction });

        const sorted = [...articles].sort((a, b) => {
            if (key === "year") {
                return direction === "asc" ? a.year - b.year : b.year - a.year;
            }
            const aVal = a[key]?.toString().toLowerCase() || "";
            const bVal = b[key]?.toString().toLowerCase() || "";
            if (aVal < bVal) return direction === "asc" ? -1 : 1;
            if (aVal > bVal) return direction === "asc" ? 1 : -1;
            return 0;
        });

        setArticles(sorted);
    };

    // Pagination helpers
    const indexOfLast = currentPage * itemsPerPage;
    const indexOfFirst = indexOfLast - itemsPerPage;
    const currentItems = articles.slice(indexOfFirst, indexOfLast);
    const totalPages = Math.ceil(articles.length / itemsPerPage);

    return (
        <div>
            <div className="header">
                <h1>InsightHub</h1>
                <p className="subtitle">Unlock your curiosity, search for scientific articles here</p>
            </div>

            <div className="search-bar-wrapper">
                <div className="search-bar">
                    <input
                        type="text"
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        onKeyDown={(e) => {
                            if (e.key === "Enter") handleSearch();
                        }}
                        placeholder="Search for articles..."
                    />
                    <button onClick={handleSearch}>Search</button>
                </div>
            </div>

            <div className="container">
                <div className="sidebar">
                    <h3>Previous Searches</h3>
                    <ul>
                        {searchHistory.map((q, idx) => (
                            <li key={idx} onClick={() => { setQuery(q); handleSearch(); }}>{q}</li>
                        ))}
                    </ul>
                </div>

                <div className="main-content">
                    <table className="article-table">
                        <thead>
                            <tr>
                                <th onClick={() => handleSort("title")} style={{ cursor: "pointer" }}>Title</th>
                                <th onClick={() => handleSort("authors")} style={{ cursor: "pointer" }}>Authors</th>
                                <th onClick={() => handleSort("year")} style={{ cursor: "pointer" }}>Year</th>
                                <th onClick={() => handleSort("publisher")} style={{ cursor: "pointer" }}>Publisher</th>
                                <th>Favourited</th>
                            </tr>
                        </thead>
                        <tbody>
                            {currentItems.map((article, idx) => (
                                <tr key={idx} className={`article-row ${idx % 2 === 0 ? "even" : "odd"}`}>
                                    <td>{article.title}</td>
                                    <td>{article.authors?.join(", ")}</td>
                                    <td>{article.year}</td>
                                    <td>{article.publisher}</td>
                                    <td>
                                        <input
                                            type="checkbox"
                                            checked={favorites.includes(article.title)}
                                            onChange={() => toggleFavorite(article.title)}
                                        />
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    {articles.length > itemsPerPage && (
                        <div className="pagination-controls">
                            <button onClick={() => setCurrentPage(p => Math.max(p - 1, 1))} disabled={currentPage === 1}>
                                Previous
                            </button>
                            <span>Page {currentPage} of {totalPages}</span>
                            <button onClick={() => setCurrentPage(p => Math.min(p + 1, totalPages))} disabled={currentPage === totalPages}>
                                Next
                            </button>
                        </div>
                    )}
                </div>

                <div className="sidebar">
                    <h3>Favorites</h3>
                    <ul>
                        {favorites.map((title, idx) => (
                            <li key={idx}>{title}</li>
                        ))}
                    </ul>
                </div>
            </div>
        </div>
    );
}

export default App;
