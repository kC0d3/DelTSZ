import Footer from "../Components/Footer";
import Header from "../Components/Header";

export default function HomePage({ loggedUser, setLoggedUser, slides, interval }) {
    return (
        <div className="homepage">
            <Header {...{ loggedUser, setLoggedUser, slides, interval }} />
            <Footer />
        </div>
    );
}