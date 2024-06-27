import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomeBody from './Components/HomeBody';
import ErrorBody from './Components/ErrorBody';
import OurProductsBody from './Components/OurProductsBody';
import Header from './Components/Header';
import Footer from './Components/Footer';
import Login from './Components/Login';

export default function App() {
  const [loggedUser, setLoggedUser] = useState(undefined);
  const [productTypes, setProductTypes] = useState(undefined);
  const [showLogin, setShowLogin] = useState(false);
  const [showRegistration, setShowRegistration] = useState(false);

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

  const productDescriptions = {
    Paprika400G: { name: 'Paprika 400g', description: 'Best for sandwiches, ratatouille and raw consume.' },
    Tomato200G: { name: 'Tomato 200g', description: 'Best for sandwiches, ratatouille and raw consume.' },
    Tomato500G: { name: 'Tomato 500g', description: 'Best for sandwiches, ratatouille and raw consume.' },
    RatatouilleMix500G: { name: 'Ratatouille mix 500g', description: 'Best for ratatouille bases.' },
    SoupMix750G: { name: 'Soup mix 750g', description: 'Best for chicken soup bases.' },
    Carrot: { name: 'Carrot', description: 'Best for Wild sauce and soups.' },
    Celery: { name: 'Celery', description: 'Best for celery bisque and soups.' },
    Cucumber: { name: 'Cucumber', description: 'Best for sandwiches and refreshing drinks.' },
    Mushroom: { name: 'Mushroom', description: 'Best for stews and bisques.' },
    Onion: { name: 'Onion', description: 'Most of the stewes bases and extra seasoning.' },
    ParsleyRoot: { name: 'Parsley root', description: 'Best for chicken soups bases.' },
    Paprika: { name: 'Paprika', description: 'Best for sandwiches, ratatouille and raw consume.' },
    Potato: { name: 'Potato', description: 'Best for fried and mashed potato.' },
    Radish: { name: 'Radish', description: 'Best for sandwiches and raw consume.' },
    Tomato: { name: 'Tomato', description: 'Best for sandwiches, ratatouille and raw consume.' }
  };

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
      <Login {...{ setLoggedUser, showLogin, setShowLogin, setShowRegistration }} />
      <Header {...{ loggedUser, setLoggedUser, showLogin, setShowLogin, slides, slidesInterval, bgImagesAmount }} />
      <Routes>
        <Route path='/' element={<HomeBody {...{ achievementCounters, achievementDuration, achievementStart }} />} />
        <Route path='*' element={<ErrorBody />} />
        <Route path='our-products' element={<OurProductsBody {...{ productTypes, productDescriptions }} />} />
      </Routes>
      <Footer />
    </Router>
  );
}