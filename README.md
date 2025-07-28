# InsightHub  
_Academic article search made simple._

InsightHub is a full-stack web application that allows users to search for scientific articles using the Semantic Scholar API. Users can sort, favorite, and revisit previous search queries — a lightweight research assistant at your fingertips.

---

##  Tech Stack

- **Frontend**: React (Create React App)
- **Backend**: ASP.NET Core 8 Web API
- **API Source**: Semantic Scholar (unauthenticated mode)

---

##  Project Structure

```
InsightHub/
├── InsightHub.ArticleFetcher/     # ASP.NET Core backend
│   ├── Controllers/
│   ├── Services/
│   └── Models/
├── insighthub-ui/                 # React frontend
│   ├── public/
│   └── src/
│       ├── App.js
│       ├── App.css
│       └── ...
└── README.md
```

---

##  Getting Started

### 1. Backend (.NET API)

**Requirements**:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 or newer

**Steps**:
1. Open `InsightHub.sln` in Visual Studio.
2. Make sure the launch profile is set to `InsightHub.ArticleFetcher`.
3. Hit **F5** or click the green **Play** button.
4. Swagger should launch at: `https://localhost:7186/swagger/index.html`

---

### 2. Frontend (React UI)

**Requirements**:
- Node.js v18+
- npm v9+

**Steps**:
```bash
cd insighthub-ui
npm install
npm start
```

The React frontend will launch at: [http://localhost:3000](http://localhost:3000)

---

### 3. API Proxy Setup

The frontend uses a proxy to avoid CORS issues during development. This is already set in `insighthub-ui/package.json`:

```json
"proxy": "https://localhost:7186"
```

No further configuration is needed.

---

##  Features WiP

- Search articles by topic
- Results include: title, author(s), year, and publisher
- Profile setup inlcuding:
    - Favorite articles with a checkbox
    - Prefered referencing style
    - View and reuse recent searches
- Sort by title, author, year, or publisher
- Press Enter to search
- Clean, accessible layout styled in Figma
- Pagination (planned): 20 results per page

---

## Development Notes

### Recommended VS Code Extensions (for React work):
- ESLint
- Prettier
- React Developer Tools

---

## License

MIT © 2025
```
