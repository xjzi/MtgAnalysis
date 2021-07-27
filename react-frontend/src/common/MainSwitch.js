import {
  Route,
  Switch,
} from "react-router-dom";

import About from "../about/About";
import Cluster from "../cluster/Cluster";
import NotFound from "../not-found/NotFound";
import Previews from "../cardsets/Previews";
import React from "react";

export default function MainSwitch() {
    return (
      <Switch>
        <Route path="/themes/:id" component={Cluster}/>
        <Route path="/cardsets/:cardset" component={Previews}/>
        <Route exact path="/" component={About}/>
        <Route component={NotFound}/>
      </Switch>
    );
  }
