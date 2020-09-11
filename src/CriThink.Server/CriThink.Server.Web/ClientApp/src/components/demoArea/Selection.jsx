import React, { Component } from "react";
import { Link } from "react-router-dom";
import { WaveDown } from "./../Layout";
import {
  Card,
  Image,
  Icon,
  Popup
} from "semantic-ui-react";
import Book from "./../../svg/bookreader.svg";
import Check from "./../../svg/check.svg";
import Graduation from "./../../svg/graduation.svg";
import LogoRound from "./../../svg/logoround.svg";

export class SelectionArea extends Component {
  render() {
    return (
      <div>
        <WaveDown>
          <Card.Group itemsPerRow={4} className="menucard">
            <Card>
              <Card.Content>
                <p className="regular">Check Your News</p>
                <Image src={Book} />
                <Card.Content className="txtRight">
                  <Link to="/3">
                    <Popup
                      content="Check your news"
                      trigger={
                        <span className="whitelink bold">
                          CHECK <Icon name="long arrow alternate right" />
                        </span>
                      }
                    ></Popup>
                  </Link>
                </Card.Content>
              </Card.Content>
            </Card>

            <Card>
              <Card.Content>
                <p className="regular">Debunked Fake News</p>
                <Image src={Check} style={{ height: "59px" }} />
                <Card.Content className="txtRight">
                  <Popup
                    content="Section under construction..."
                    trigger={
                      <span className="nolink bold">
                        DEBUNK <Icon name="long arrow alternate right" />
                      </span>
                    }
                  ></Popup>
                </Card.Content>
              </Card.Content>
            </Card>

            <Card>
              <Card.Content>
                <p className="regular">Spot Fake News</p>
                <Image src={Graduation} />
                <Card.Content className="txtRight">
                  <Popup
                    content="Section under construction..."
                    trigger={
                      <span className="nolink bold">
                        LEARN <Icon name="long arrow alternate right" />
                      </span>
                    }
                  ></Popup>
                </Card.Content>
              </Card.Content>
            </Card>

            <Card>
              <Card.Content>
                <p className="regular">Quiz & Games</p>
                <Image src={LogoRound} />
                <Card.Content className="txtRight">
                  <Popup
                    content="Section under construction..."
                    trigger={
                      <span className="nolink bold">
                        PLAY <Icon name="long arrow alternate right" />
                      </span>
                    }
                  ></Popup>
                </Card.Content>
              </Card.Content>
            </Card>
          </Card.Group>
        </WaveDown>
      </div>
    );
  }
}
