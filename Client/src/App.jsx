import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import HomeBody from './Components/HomeBody';
import ErrorBody from './Components/ErrorBody';
import OurProducts from './Components/OurProducts';
import Header from './Components/Header';
import Footer from './Components/Footer';
import Login from './Components/Login';
import Registration from './Components/Registration';
import ProvideIngredient from './Components/ProvideIngredient';

export default function App() {
  const [loggedUser, setLoggedUser] = useState(undefined);
  const [productAndIngredientTypes, setProductAndIngredientTypes] = useState(undefined);
  const [ingredientTypes, setIngredientTypes] = useState(undefined);
  const [productTypes, setProductTypes] = useState(undefined);
  const [showLogin, setShowLogin] = useState(false);
  const [showRegistration, setShowRegistration] = useState(false);
  const [slideData, setSlideData] = useState(undefined);
  const [achievementData, setAchievementData] = useState(undefined);
  const [productDescriptions, setProductDescriptions] = useState(undefined);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchData();
  }, []);

  useEffect(() => {
    const interval = setInterval(() => {
      fetchUser();
    }, 540000);

    return () => clearInterval(interval);
  }, [loggedUser]);

  const fetchData = async () => {
    try {
      await fetchIngredientAndProductTypes();
      await fetchGlobalData();
      await fetchUser();
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchIngredientAndProductTypes = async () => {
    try {
      const prodRes = await fetch('/api/products/types');
      const prodData = await prodRes.json();

      const ingRes = await fetch('/api/ingredients/types');
      const ingData = await ingRes.json();

      const combinedData = [...prodData, ...ingData];
      setProductAndIngredientTypes(combinedData);
      setIngredientTypes(ingData);
      setProductTypes(prodData);
    } catch (error) {
      console.log(error);
    }
  }

  const fetchGlobalData = async () => {
    try {
      await fetchSlides();
      await fetchAchievements();
      await fetchProductDescriptions();
    } catch (error) {
      console.log(error);
    }
  }

  const fetchSlides = async () => {
    try {
      const slidesRes = await fetch('/api/global/data/slides');
      const slidesData = await slidesRes.json();
      setSlideData(slidesData);
    } catch (error) {
      console.log(error);
    }
  }

  const fetchAchievements = async () => {
    try {
      const achievementsRes = await fetch('/api/global/data/achievements');
      const achievementsData = await achievementsRes.json();
      setAchievementData(achievementsData);
    } catch (error) {
      console.log(error);
    }
  }

  const fetchProductDescriptions = async () => {
    try {
      const productDescriptionsRes = await fetch('/api/global/data/product-descriptions');
      const productDescriptionsData = await productDescriptionsRes.json();
      setProductDescriptions(productDescriptionsData);
    } catch (error) {
      console.log(error);
    }
  }

  const fetchUser = async () => {
    try {
      const userRes = await fetch('/api/users');
      const userData = await userRes.json();

      setLoggedUser(userData);
    } catch (error) {
      console.log(error);
      setLoggedUser(undefined);
    }
  }

  if (isLoading) {
    return <></>;
  }

  return (
    <Router>
      <ToastContainer />
      <Login {...{ fetchUser, showLogin, setShowLogin, setShowRegistration }} />
      <Registration {...{ showRegistration, setShowRegistration, setShowLogin }} />
      <Header {...{ loggedUser, setLoggedUser, showLogin, setShowLogin, slideData }} />
      <Routes>
        <Route path='/' element={<HomeBody {...{ achievementData }} />} />
        <Route path='*' element={<ErrorBody />} />
        <Route path='our-products' element={<OurProducts {...{ productAndIngredientTypes, productDescriptions }} />} />
        <Route path='/ingredients/provide' element={<ProvideIngredient {...{ ingredientTypes, loggedUser, setShowLogin }} />} />
      </Routes>
      <Footer />
    </Router>
  );
}