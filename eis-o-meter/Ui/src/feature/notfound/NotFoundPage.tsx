    import {Link} from "react-router";
    import NotFoundImage from '../../assets/404.svg';
    
    function NotFoundPage() {
        return (
            <>
                <section className="space-y-20">
                    <div className="relative flex flex-col-reverse items-center justify-center md:flex-row" id="hero">
                        <div className="row items-center py-5 md:pb-20 md:pt-10">
                            <div className="justify-center flex py-5">
                                <img src={NotFoundImage} alt="404 Not Found" className="w-1/2 h-auto" />
                            </div>
                            <div className="text-center space-y-10">
                                <h2 className="text-2xl font-medium leading-none md:text-4xl">Sorry, we couldn't find the
                                    page you were looking for</h2>
                                <Link to="/">
                                <a className="px-9 py-5 bg-black hover:bg-white text-white hover:text-black border rounded-2xl justify-items-center md:justify-items-start gap-2.5 inline-flex">
                                    <h2 className="text-center text-xl font-normal leading-7">Go Back Home</h2>
                                </a>
                                </Link>
                            </div>  
                        </div>
                    </div>
                </section>
            </>
        )
    }
    
    export default NotFoundPage
