import React, { useEffect, useState } from 'react';

import Card from '../common/Card'
import CardInfo from '../card-info/CardInfo';
import DataHandler from '../common/DataHandler';
import FetchJson from '../common/FetchJson';
import styles from './Cluster.module.css';
import { useParams } from "react-router-dom";

export default function Cluster(){
  const [error, setError] = useState(false);
  const [deck, setDeck] = useState(null);
  const [currentCard, setCurrentCard] = useState(null);
  const { id } = useParams();

  useEffect(() => {
    async function fetchDeck(){
      const json = await FetchJson(`/api/cluster/${id}`);
      if(json.length > 0){
        setDeck(json[0]);
      } else {
        console.error(`Cluster ${id} does not exist.`);
        setError(true);
      }
    }
    fetchDeck();
  }, [id]);

  const render = () => {
    function Collapse(data) {
      return data.reduce(function(storage, item) {
        storage[item] = storage[item] || 0;
        storage[item]++;
        return storage; 
      }, {});
    };

    function Cards(cards){
      const names = Object.keys(cards);
      return names.map((name) => {
        return(
          <p key={name} onMouseEnter={() => setCurrentCard(name)} >
            <Card card={name} frequency={cards[name]}/>
          </p>
        );
      });
    }

    return(
      <div>
        <h1>Deck Details</h1>
        <div className={styles['info-box']}>
          <div className={styles['board']}>
            <h2>Mainboard</h2>
            {Cards(Collapse(deck.mainboard))}
          </div>
          {
            currentCard && 
            <div className={styles['card-info']}>
              <h2>Card Details</h2>
              <CardInfo card={currentCard}/>
            </div>
          }
          <div className={styles['board']}>
            <h2>Sideboard</h2>
            {Cards(Collapse(deck.sideboard))}
          </div>
        </div>
      </div>
    );
  }

  return <DataHandler error={error} data={deck} render={render}/>;
}