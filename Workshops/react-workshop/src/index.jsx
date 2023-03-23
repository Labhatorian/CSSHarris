// Vendors
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { QueryClientProvider } from 'react-query';
import { BrowserRouter } from 'react-router-dom';

// Components
import App from './App';

// Other
import queryClient from './Queries/client';

// Styling
import './index.css';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <StrictMode>
    <QueryClientProvider client={ queryClient }>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </QueryClientProvider>
  </StrictMode>
);
