import React, { useEffect, useState } from 'react';

import DataHandler from '../common/DataHandler';
import FetchJson from '../common/FetchJson';
import { Link } from "react-router-dom";
import styles from './Links.module.css';

export default function Links(){
  const [error, setError] = useState(false);
  const [cardsets, setCardsets] = useState([]);

  useEffect(() => {
    async function fetchCardsets(){
      const json = await FetchJson('/api/cardsets');
      if(json.length > 0){
        setCardsets(json.map((cardset) => cardset.cardset));
      } else {
        console.error(`No cardsets were found.`);
        setError(true);
      }
    }
    fetchCardsets();
  }, []);

  const render = () => {
    const individualLinks = [ { url: "/", text: "Home" } ];
    const cardsetLinks = cardsets.map((cardset) => 
      ({ url: `/cardsets/${cardset}`, text: cardset.charAt(0).toUpperCase() + cardset.slice(1) })
    );

    const linkElements = individualLinks.concat(cardsetLinks)
      .map((link) => <Link to={link.url} key={link.text} className={styles['link']}>{link.text}</Link>)

    return(
      <div className={styles['link-box']}>
        {linkElements}
      </div>
    );
  }

  return <DataHandler error={error} data={cardsets} render={render}/>
}