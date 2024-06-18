import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ErrorPage from './Pages/ErrorPage';
import HomePage from './Pages/HomePage';

export default function App() {
  const [loggedUser, setLoggedUser] = useState(undefined);
  const bgImagesAmount = 3;
  const interval = 6500;
  const slides = [{
    img: '/images/bg1.jpg',
    bubble: {
      h1: 'Local Champions',
      p1: 'Fresh, juicy and healthy',
      p2: 'local vegetables every day of the year.',
      button: 'Our Products'
    }
  },
  {
    img: '/images/bg2.jpg',
    bubble: {
      h1: 'Local producers',
      p1: 'Our producers provide reliable',
      p2: 'fresh, juicy and healthy vegetables.',
      button: 'Our Products'
    }
  },
  {
    img: '/images/bg3.jpg',
    bubble: {
      h1: 'DelTSZ',
      p1: 'Our logistic center',
      p2: 'can handle more than 1.000 orders every day.',
      button: 'Our Products'
    }
  }];

  return (
    <Router>
      <Routes>
        <Route path='/' element={<HomePage {...{ loggedUser, setLoggedUser, slides, interval }} />} />
        <Route path='*' element={<ErrorPage {...{ loggedUser, setLoggedUser, bgImagesAmount }} />} />
      </Routes>
    </Router>
  );
}