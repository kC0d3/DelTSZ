import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ErrorPage from './Pages/ErrorPage';
import HomePage from './Pages/HomePage';
import OurProductsPage from './Pages/OurProductsPage';

export default function App() {
  const [loggedUser, setLoggedUser] = useState(undefined);
  const [productTypes, setProductTypes] = useState(undefined);

  const bgImagesAmount = 3;
  const slidesInterval = 6500;
  const slides = [{
    img: '/images/backgrounds/bg1.jpg',
    bubble: {
      h1: 'Local Champions',
      p1: 'Fresh, juicy and healthy',
      p2: 'local vegetables every day of the year.',
      button: 'Our Products'
    }
  },
  {
    img: '/images/backgrounds/bg2.jpg',
    bubble: {
      h1: 'Local producers',
      p1: 'Our producers provide reliable',
      p2: 'fresh, juicy and healthy vegetables.',
      button: 'Our Products'
    }
  },
  {
    img: '/images/backgrounds/bg3.jpg',
    bubble: {
      h1: 'DelTSZ',
      p1: 'Our logistic center',
      p2: 'can handle more than 1.000 orders every day.',
      button: 'Our Products'
    }
  }];

  const achievementDuration = 2000;
  const achievementStart = 0;
  const achievementCounters = [
    {
      amount: 500,
      achievement: 'Producer',
      description: 'We operate together with that much local producer.'
    },
    {
      amount: 53000,
      achievement: 'Tons',
      description: 'We can provide that much healty vegetables and support your health every year.'
    },
    {
      amount: 150,
      achievement: 'Hectar greenhouse',
      description: 'Around that much area available for driven cultivation.'
    },
    {
      amount: 96,
      achievement: 'Biological plant protection',
      description: 'We say yes to sustainability and environmental consciousness.'
    }
  ];

  useEffect(() => {
    fetchProductTypes();
  }, []);

  const fetchProductTypes = async () => {
    try {
      const prodRes = await fetch('/api/products/types');
      const prodData = await prodRes.json();

      const ingRes = await fetch('/api/ingredients/types');
      const ingData = await ingRes.json();

      const combinedData = [...prodData, ...ingData];
      setProductTypes(combinedData);
    } catch (error) {
      console.log("Error fetching data:", error);
    }
  }

  return (
    <Router>
      <Routes>
        <Route path='/' element={<HomePage {...{ loggedUser, setLoggedUser, slides, slidesInterval, achievementCounters, achievementDuration, achievementStart }} />} />
        <Route path='*' element={<ErrorPage {...{ loggedUser, setLoggedUser, bgImagesAmount }} />} />
        <Route path='our-products' element={<OurProductsPage {... { loggedUser, setLoggedUser, bgImagesAmount, productTypes, productDescriptions }} />} />
      </Routes>
    </Router>
  );
}