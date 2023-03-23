import { Routes, Route } from 'react-router';
import GalleryContainer from './Components/Containers/GalleryContainer';
import DetailContainer from './Components/Containers/DetailContainer';
import './App.css';

const App = () => {
  return (
    <div className="app">
      <Routes>
        <Route path="/" element={<GalleryContainer />} />
        {/* Here we'll want to add a route to see the details page. Preferably, we use the 'id' of the planet in the route. */}
        <Route path="detail/:planetId" element={<DetailContainer />} />
      </Routes>
    </div>
  );
}

export default App;
