// svg 内的JS 源码

var _top = top.svgweb ? top : window.parent;
var SVG = "http://www.w3.org/2000/svg";
var XLINK = "http://www.w3.org/1999/xlink";
function loaded() {
    // change onloadFunc to point to your real onload function that you
    // want called when the page is truly ready
    var onloadFunc = doload;
    if (_top.svgweb != undefined) {
        _top.svgweb.addOnLoad(onloadFunc, true, window);
    } else {
        onloadFunc();
    }
}
function FamilyGraph(options) {
    this.throwError = function () {
        if (!arguments.length) return false;
        var s = "";
        for (var i = 0; i < arguments.length; i++) {
            s = s + ", " + arguments[i];
        }
        if (console.trace) {
            s = s + ", function stack : " + console.trace();
        }
        if (this.settings.debug) {
            throw new Error(s);
        }
        else {
            console.log(s);
        }
    }
    var self = this;
    this.domOptions = options.domOptions;
    this.tree = null;
    this.treeDom = function () {
        if (!this.tree) {
            this.tree = document.getElementById("containertree");
        }
        return this.tree;
    };
    this.relationGroup = null;
    this.tooltipGroup = null;
    this.settings = _top.FG.extend({}, _top.FG.defaultSettings, options.settings);

    this.events = options.events;

    this._peoples = options.peoples;
    this.peoples = [];
    this._relations = options.relations;
    this.relations = [];
    this.DataPrapared = false;
    this.flagID = options.flagID;
    this.loginID = options.loginID;
    this.topLevel = 1;
    this.bottomLevel = 0;

    this.flagPeople = null;
    this.getFlag = function () {
        if (!this.flagPeople) {
            this.flagPeople = this.getPeople(this.flagID);
            if (this.flagPeople.Gender != this.settings.Gender.Male && this.flagPeople.Husband) {
                this.flagPeople = this.flagPeople.Husband;
            }
        }
        return this.flagPeople;
    };

    this._mySelf = null;
    this.getMySelf = function () {
        if (!this._mySelf) {
            this._mySelf = this.getPeople(this.loginID);
        }
        return this._mySelf;
    };

    this.peopleTree = [];
    this.getPeople = function (id) {
        var people = 0;
        for (var i = 0; i < this.peoples.length; i++) {
            if (this.peoples[i].ID == id) {
                people = this.peoples[i];
                break;
            }
        }
        return people;
    };

    // 首先把每个人相互之间的引用加上
    this.addReferenceForPeople = function () {
        for (var i = 0; i < this.relations.length; i++) {
            var relation = this.relations[i];
            var sourcePeople = relation.getSourcePeople();
            var targetPeople = relation.getTargetPeople();
            if (!sourcePeople || !targetPeople) { continue; }

            switch (relation.Relation) {
                case "Father":
                    sourcePeople.AddChild(targetPeople);
                    targetPeople.Father = sourcePeople;
                    break;
                case "Mother":
                    sourcePeople.AddChild(targetPeople);
                    targetPeople.Mother = sourcePeople;
                    break;
                case "Husband":
                    sourcePeople.Wife = targetPeople;
                    targetPeople.Husband = sourcePeople;
                    break;
                case "Wife":
                    sourcePeople.Husband = targetPeople;
                    targetPeople.Wife = sourcePeople;
                    break;
                default: break;
            }
        }
        // 处理小孩有父母 但是父母不是夫妻关系的情况
        for (var i = 0; i < this.peoples.length; i++) {
            var child = this.peoples[i];
            if (child.Father && child.Mother && !child.Father.Wife && !child.Mother.Husband) {
                child.Father.Wife = child.Mother;
                child.Mother.Husband = child.Father;
                this.relations.push(_top.FG.createRelation({
                    SourceID: child.Father.ID,
                    Relation: "Husband",
                    TargetID: child.Mother.ID
                }));
            }
        }
    }
    // 更新每个人的Level
    this.ResetLevel = function (people, initLevel) {
        if (!people.levelFlag) {
            people.Level = initLevel;
            people.levelFlag = true;
        }
        if (people.Wife && !people.Wife.levelFlag) {
            this.ResetLevel(people.Wife, people.Level);
        }
        if (people.Husband && !people.Husband.levelFlag) {
            this.ResetLevel(people.Husband, people.Level);
        }
        if (people.Father && !people.Father.levelFlag) {
            this.ResetLevel(people.Father, people.Level + 1);
        }
        if (people.Mother && !people.Mother.levelFlag) {
            this.ResetLevel(people.Mother, people.Level + 1);
        }
        if (people.Children && people.Children.length) {
            for (var i = 0; i < people.Children.length; i++) {
                if (!people.Children[i].levelFlag) {
                    this.ResetLevel(people.Children[i], people.Level - 1);
                }
            }
        }
    }

    this.PrapareData = function () {
        if (this.DataPrapared)
            return true;
        if (!this.peoples || this.peoples.length == 0)
            this.throwError("Data error", "Initialize data error.");

        // step 1
        this.addReferenceForPeople();

        // step 2
        this.ResetLevel(this.peoples[0], 0);

        // step 3
        this.peoples = _top.FG.quickSortByLevel(this.peoples.slice(0));

        // step 4
        this.topLevel = this.peoples[0].Level;
        this.bottomLevel = this.peoples[this.peoples.length - 1].Level;
        this.Levels = this.topLevel - this.bottomLevel + 1;

        // step 5 绘制数据源
        this.peopleTree = [];
        // 第一级数据源
        this.peopleTree.push([]);
        for (var j = 0; j < this.peoples.length; j++) {
            if (this.peoples[j].Level == this.topLevel && (this.peoples[j].Gender == this.settings.Gender.Male || !this.peoples[j].Husband)) {
                var people = this.peoples[j];
                var newPeople = { ID: people.ID };
                if (people.Wife) {
                    newPeople.Wife = { ID: people.Wife.ID };
                }
                // 根据 flag 调整祖先顺序
                if (this.getFlag().ISMyGrandF(people) || this.getFlag().ISMyGrandM(people)) {
                    this.peopleTree[0].unshift(newPeople);
                }
                else {
                    this.peopleTree[0].push(newPeople);
                }
            }
        }
        // 更新父亲节点的MaxChildrenCount
        for (var i = 0; i < this.peopleTree[0].length; i++) {
            var tPeople = this.getPeople(this.peopleTree[0][i].ID);
            tPeople.UpdateMaxChildrenCount();
        }
        // 其他级别数据源
        for (var i = 1; i < this.Levels; i++) {
            this.peopleTree.push([]);
            for (var j = 0; j < this.peopleTree[i - 1].length; j++) {
                var tPeople = this.getPeople(this.peopleTree[i - 1][j].ID);
                var _childRen = tPeople.getChildren();
                if (!_childRen) { continue; }
                for (var k = 0; k < _childRen.length; k++) {
                    var people = { ID: _childRen[k].ID };
                    if (_childRen[k].Wife) {
                        people.Wife = { ID: _childRen[k].Wife.ID };
                    }
                    this.peopleTree[i].push(people);
                }
            }
        }

        this.DataPrapared = true;
    }
    this.createRect = function (x, y, width, height, others) {
        var obj = document.createElementNS(SVG, "rect");
        obj.setAttribute("x", x);
        obj.setAttribute("y", y);
        obj.setAttribute("width", width);
        obj.setAttribute("height", height);
        for (var attr in others) {
            obj.setAttribute(attr.replace("___", "").replace("__", "-"), others[attr]);
        }
        return obj;
    }
    this.createImage = function (src, x, y, width, height, others) {
        var obj = document.createElementNS(SVG, 'image');
        try {
            if (src && src.indexOf("http") < 0) {
                src = _top.document.location.protocol + "//" + _top.document.location.host + src;
            }
            obj.setAttributeNS(XLINK, "xlink:href", src);
        }
        catch (e) {
            obj.setAttribute('src', src);
        }
        obj.setAttribute("x", x);
        obj.setAttribute("y", y);
        obj.setAttribute("width", width);
        obj.setAttribute("height", height);
        for (var attr in others) {
            obj.setAttribute(attr.replace("___", "").replace("__", "-"), others[attr]);
        }

        return obj;
    }
    this.createText = function (text, x, y, width, height, others) {
        var textN = document.createTextNode(text, true);
        var obj = document.createElementNS(SVG, "text");
        obj.appendChild(textN);
        obj.setAttribute("x", x);
        obj.setAttribute("y", y);
        obj.setAttribute("width", width);
        obj.setAttribute("height", height);
        for (var attr in others) {
            obj.setAttribute(attr.replace("___", "").replace("__", "-"), others[attr]);
        }
        return obj;
    }
    this.createAnimateTran = function () {
        var animateTran = document.createElementNS(SVG, "animateTransform");
        animateTran.setAttribute("attributeName", "transform");
        animateTran.setAttribute("type", "translate");
        animateTran.setAttribute("values", "0 -300;0 -20;0 0");
        animateTran.setAttribute("keyTimes", "0;0.5;1");
        animateTran.setAttribute("begin", "0s");
        animateTran.setAttribute("dur", "600ms");
        animateTran.setAttribute("fill", "freeze");
        return animateTran;
    }
    this.createSet = function (sourceID) {
        //<set attributeName="visibility" from="hidden" to="visible" begin="thingyouhoverover.mouseover" end="thingyouhoverover.mouseout"/>
        var animateTran = document.createElementNS(SVG, "set");
        animateTran.setAttribute("attributeName", "visibility");
        animateTran.setAttribute("from", "hidden");
        animateTran.setAttribute("to", "visible");
        animateTran.setAttribute("begin", sourceID + ".mouseover");
        animateTran.setAttribute("end", sourceID + ".mouseout");
        return animateTran;
    }
    this.createTooltipBG = function (buttonID, x, y, width, height, others) {
        var action1Tooltip_Bg = this.createRect(x, y, width, height, others);
        var action1Tooltip_Animate = this.createSet(buttonID);
        action1Tooltip_Bg.appendChild(action1Tooltip_Animate);
        return action1Tooltip_Bg;
    }
    this.createTooltipText = function (buttonID, title, x, y, width, height, others) {
        var action1Tooltip_Text = this.createText(title, x, y,
            width, height, others);
        var action1Tooltip_Animate2 = this.createSet(buttonID);
        action1Tooltip_Text.appendChild(action1Tooltip_Animate2);
        return action1Tooltip_Text;
    }
    this.drawPeople = function (people) {
        var g = this.createSVGGroupIFNo("people" + people.ID);
        g.appendChild(this.createAnimateTran());
        this.treeDom().appendChild(g);

        // people card
        var color = this.settings.color.Male;
        if (people.Gender == this.settings.Gender.Female) { color = this.settings.color.Female; }
        if (people.Gender == this.settings.Gender.None) { color = this.settings.color.None; }
        if (people.Status == "Requesting") { color = this.settings.color.Requesting };
        if (people.ID == this.flagID) { color = this.settings.color.Self; }

        var rect = this.createRect(people.x, people.y, people.width, people.height,
        {
            fill: color,
            class___: "people",
            _data: people.ID,
            rx: this.settings.size.rx,
            ry: this.settings.size.ry,
            id: "people" + people.ID
        });

        g.appendChild(rect);
        // people logo
        var strStyle = "cursor:pointer";
        if (people.Status == "VirtualAccount" || people.ID == this.flagID) { strStyle = ""; }
        var imgLogo = new Image();
        imgLogo.onload = function (event) {
            var logo = self.createImage(people.LittleHeadImage,
            people.x + self.settings.attribute.Logo.Left, people.y + self.settings.attribute.Logo.Top,
            self.settings.attribute.Logo.Width, self.settings.attribute.Logo.Height,
            {
                class___: "logo",
                _data: people.ID,
                id: "people" + people.ID + "logo",
                style: strStyle
            });
            logo.addEventListener("mousedown", function (event) {
                if (event.button != 0)
                    return false;
                var people = self.getPeople(event.target.getAttribute("_data"));
                self.events.peopleLinkClicked(people, event.target);
            }, false);
            g.appendChild(logo);
        }
        imgLogo.src = people.LittleHeadImage;

        var logoBorder = this.createRect(people.x + this.settings.attribute.Logo.Left - this.settings.attribute.Logo.Border.Width,
        people.y + this.settings.attribute.Logo.Top - this.settings.attribute.Logo.Border.Width,
        this.settings.attribute.Logo.Width + this.settings.attribute.Logo.Border.Width * 2,
        this.settings.attribute.Logo.Height + this.settings.attribute.Logo.Border.Width * 2,
        {
            stroke: this.settings.attribute.Logo.Border.Color,
            fill: "none",
            stroke__width: this.settings.attribute.Logo.Border.Width
        });
        g.appendChild(logoBorder);

        // name
        var nameNode = this.createText(people.Name, people.x + this.settings.attribute.Name.Left, people.y + this.settings.attribute.Name.Top,
                                        this.settings.attribute.Name.Width, this.settings.attribute.Name.Height, {
                                            alignment__baseline: "baseline",
                                            id: "people" + people.ID + "Name",
                                            class___: "name",
                                            fill: this.settings.attribute.Name.Color
                                        });
        g.appendChild(nameNode);
        if (people.Status != "Requesting") {
            // Birthday
            var birthNode = this.createText(people.Birthday, people.x + this.settings.attribute.BirthDay.Left, people.y + this.settings.attribute.BirthDay.Top,
                                            this.settings.attribute.BirthDay.Width, this.settings.attribute.BirthDay.Height, {
                                                alignment__baseline: "baseline",
                                                id: "people" + people.ID + "Birthday",
                                                class___: "birthday",
                                                fill: this.settings.attribute.BirthDay.Color
                                            });
            g.appendChild(birthNode);
        }
        // 查看他人的树,无任何操作
        if (this.flagID != this.loginID) { return true; }

        if (people.Status != "Requesting") {
            // action: add Member
            if (typeof people.buttonCount == "undefined") { people.buttonCount = 0 };
            var x = people.x + this.settings.attribute.Action.Left + (this.settings.attribute.Action.Width + this.settings.attribute.Action.Space) * people.buttonCount;

            var action1 = this.createImage("/images/icons/treeadd.png", x, people.y + this.settings.attribute.Action.Top,
            this.settings.attribute.Action.Width, this.settings.attribute.Action.Height, {
                id: "people" + people.ID + "ActionAdd",
                class___: "action",
                _data: people.ID,
                style: "cursor:pointer"
            });
            action1.addEventListener("mousedown", function (event) {
                if (event.button != 0)
                    return false;
                var people = self.getPeople(event.target.getAttribute("_data"));
                self.events.peopleAddClicked(people, event.target);
            }, false);
            g.appendChild(action1);

            var action1Tooltip_Bg = this.createTooltipBG("people" + people.ID + "ActionAdd", x + this.settings.attribute.Tooltip.Left,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top,
                this.settings.attribute.Tooltip.Width, this.settings.attribute.Tooltip.Height, {
                    stroke: this.settings.attribute.Tooltip.Border,
                    fill: this.settings.attribute.Tooltip.Background,
                    stroke__width: 1,
                    visibility: "hidden",
                    id: "people" + people.ID + "ActionAdd_Tooltip"
                });
            this.tooltipGroup.appendChild(action1Tooltip_Bg);

            var action1Tooltip_Text = this.createTooltipText("people" + people.ID + "ActionAdd", "Add family member", x + this.settings.attribute.Tooltip.Left + 1,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top + 15 + 1,
                this.settings.attribute.Tooltip.Width - 2, this.settings.attribute.Tooltip.Height - 2, {
                    fill: this.settings.attribute.Tooltip.Fill,
                    visibility: "hidden",
                    font__size: this.settings.attribute.Tooltip.FontSize
                });
            this.tooltipGroup.appendChild(action1Tooltip_Text);

            people.buttonCount++;
        }
        if (people.Status == "VirtualAccount") {
            if (typeof people.buttonCount == "undefined") { people.buttonCount = 0 };
            x = people.x + this.settings.attribute.Action.Left + (this.settings.attribute.Action.Width + this.settings.attribute.Action.Space) * people.buttonCount;
            var action2 = this.createImage("/images/icons/treeedit.png",
            x,
            people.y + this.settings.attribute.Action.Top,
            this.settings.attribute.Action.Width, this.settings.attribute.Action.Height, {
                id: "people" + people.ID + "ActionEdit", class___: "action", _data: people.ID, style: "cursor:pointer"
            });
            action2.addEventListener("mousedown", function (event) {
                if (event.button != 0)
                    return false;
                var people = self.getPeople(event.target.getAttribute("_data"));
                self.events.peopleEditClicked(people, event.target);
            }, false);
            g.appendChild(action2);

            var action2Tooltip_Bg = this.createTooltipBG("people" + people.ID + "ActionEdit", x + this.settings.attribute.Tooltip.Left,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top,
                this.settings.attribute.Tooltip.Width, this.settings.attribute.Tooltip.Height, {
                    stroke: this.settings.attribute.Tooltip.Border,
                    fill: this.settings.attribute.Tooltip.Background,
                    stroke__width: 1,
                    visibility: "hidden",
                    id: "people" + people.ID + "ActionEdit_Tooltip"
                });
            this.tooltipGroup.appendChild(action2Tooltip_Bg);

            var action2Tooltip_Text = this.createTooltipText("people" + people.ID + "ActionEdit", "Edit profiles", x + this.settings.attribute.Tooltip.Left + 1,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top + 15 + 1,
                this.settings.attribute.Tooltip.Width - 2, this.settings.attribute.Tooltip.Height - 2, {
                    fill: this.settings.attribute.Tooltip.Fill,
                    visibility: "hidden",
                    font__size: this.settings.attribute.Tooltip.FontSize
                });
            this.tooltipGroup.appendChild(action2Tooltip_Text);

            people.buttonCount++;
        }
        if (people.Status == "Requesting" || people.ID != this.flagID &&
            (people.ID === (this.getFlag().Wife && this.getFlag().Wife.ID) || people.ID === this.getFlag().ID
            || (!people.Children || !people.Children.length ||
                    (people.Children.length == 1
                        && (people.Children[0].ID == this.flagID ||
                            (this.getFlag().Wife && this.getFlag().Wife.ID == people.Children[0].ID)
                            ))
                )
            )) {
            if (typeof people.buttonCount == "undefined") { people.buttonCount = 0 };
            x = people.x + this.settings.attribute.Action.Left + (this.settings.attribute.Action.Width + this.settings.attribute.Action.Space) * people.buttonCount;
            var action21 = this.createImage("/images/icons/treedelete.png",
            x,
            people.y + this.settings.attribute.Action.Top,
            this.settings.attribute.Action.Width, this.settings.attribute.Action.Height, {
                id: "people" + people.ID + "ActionDelete", class___: "action", _data: people.ID, style: "cursor:pointer"
            });
            action21.addEventListener("mousedown", function (event) {
                if (event.button != 0)
                    return false;
                var people = self.getPeople(event.target.getAttribute("_data"));
                self.events.peopleDeleteClicked(people, event.target);
            }, false);
            g.appendChild(action21);

            var action21Tooltip_Bg = this.createTooltipBG("people" + people.ID + "ActionDelete", x + this.settings.attribute.Tooltip.Left,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top,
                this.settings.attribute.Tooltip.Width, this.settings.attribute.Tooltip.Height, {
                    stroke: this.settings.attribute.Tooltip.Border,
                    fill: this.settings.attribute.Tooltip.Background,
                    stroke__width: 1,
                    visibility: "hidden",
                    id: "people" + people.ID + "ActionEdit_Tooltip"
                });
            this.tooltipGroup.appendChild(action21Tooltip_Bg);

            var action21Tooltip_Text = this.createTooltipText("people" + people.ID + "ActionDelete", "Delete family member", x + this.settings.attribute.Tooltip.Left + 1,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top + 15 + 1,
                this.settings.attribute.Tooltip.Width - 2, this.settings.attribute.Tooltip.Height - 2, {
                    fill: this.settings.attribute.Tooltip.Fill,
                    visibility: "hidden",
                    font__size: this.settings.attribute.Tooltip.FontSize
                });
            this.tooltipGroup.appendChild(action21Tooltip_Text);

            people.buttonCount++;
        }
        if (people.Status != "Requesting") {
            if (people.Status == "Normal" && people.ID != this.flagID) {
                if (typeof people.buttonCount == "undefined") { people.buttonCount = 0 };
                x = people.x + this.settings.attribute.Action.Left + (this.settings.attribute.Action.Width + this.settings.attribute.Action.Space) * people.buttonCount;
                var action4 = this.createImage("/images/icons/viewtree.png",
                x,
                people.y + this.settings.attribute.Action.Top,
                this.settings.attribute.Action.Width, this.settings.attribute.Action.Height, {
                    id: "people" + people.ID + "ActionViewTree", class___: "action", _data: people.ID, style: "cursor:pointer"
                });
                action4.addEventListener("mousedown", function (event) {
                    if (event.button != 0)
                        return false;
                    var people = self.getPeople(event.target.getAttribute("_data"));
                    self.events.peopleViewTreeClicked(people, event.target);
                }, false);
                g.appendChild(action4);

                var action4Tooltip_Bg = this.createTooltipBG("people" + people.ID + "ActionViewTree", x + this.settings.attribute.Tooltip.Left,
                people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top,
                this.settings.attribute.Tooltip.Width, this.settings.attribute.Tooltip.Height, {
                    stroke: this.settings.attribute.Tooltip.Border,
                    fill: this.settings.attribute.Tooltip.Background,
                    stroke__width: 1,
                    visibility: "hidden",
                    id: "people" + people.ID + "ActionEdit_Tooltip"
                });
                this.tooltipGroup.appendChild(action4Tooltip_Bg);

                var action4Tooltip_Text = this.createTooltipText("people" + people.ID + "ActionViewTree", "View " + people.FirstName + "'s tree", x + this.settings.attribute.Tooltip.Left + 1,
                    people.y + this.settings.attribute.Action.Top + this.settings.attribute.Tooltip.Top + 15 + 1,
                    this.settings.attribute.Tooltip.Width - 2, this.settings.attribute.Tooltip.Height - 2, {
                        fill: this.settings.attribute.Tooltip.Fill,
                        visibility: "hidden",
                        font__size: this.settings.attribute.Tooltip.FontSize
                    });
                this.tooltipGroup.appendChild(action4Tooltip_Text);
                people.buttonCount++;
            }
        }

    }
    this.createPeopleRect = function (people, prevPeople) {
        var _people = this.getPeople(people.ID);
        var _wife;
        var _husband;
        if (!_people.drawed) {
            prevPeople = prevPeople.Wife || prevPeople;
            if (prevPeople.Husband
                && prevPeople.Husband.drawed
                && prevPeople.Husband.x > prevPeople.x
                ) {
                prevPeople = prevPeople.Husband;
            }
            if (people.Wife
                && (_people.Level > this.getFlag().Level
                || _people.ID == this.flagID || _people.ID == this.getFlag().ID
                || _people.Wife.ID == this.flagID || _people.Wife.ID == this.getFlag().ID)) {
                _wife = this.getPeople(people.Wife.ID);
                _people.Wife = _wife;
                _people.intervalX = this.settings.size.intervalX * 0.6;
            }
            if (_people.Husband && !_people.Husband.drawed
                && (_people.Level > this.getFlag().Level
                || _people.ID == this.flagID || _people.ID == this.getFlag().ID
                || _people.Husband.ID == this.flagID || _people.Husband.ID == this.getFlag().ID)) {
                _husband = _people.Husband;
                _people.intervalX = this.settings.size.intervalX * 0.6;
            }
            _people.x = prevPeople.x + prevPeople.width + prevPeople.intervalX;
            _people.y = this.settings.defaultPrev.y + (this.topLevel - _people.Level) * (this.settings.size.peopleHeight + this.settings.size.intervalY);
            _people.width = this.settings.size.peopleWidth;
            _people.height = this.settings.size.peopleHeight;
            _people.intervalX = _people.intervalX || this.settings.size.intervalX;
            _people.intervalY = this.settings.size.intervalY;

            if (_people.MaxChildrenCount > 1) {
                // 配置父节点偏移，孩子留出位置
                var maxWidth = _people.MaxChildrenCount * (this.settings.size.peopleWidth + this.settings.size.intervalX) - this.settings.size.intervalX;
                var adjust = (maxWidth - _people.intervalX - this.settings.size.peopleWidth * ((_people.Wife || _people.Husband) ? 2 : 1)) / 2;
                _people.x += adjust
                if (_wife) {
                    _wife.intervalX = this.settings.size.intervalX + adjust;
                }
                else if (_husband) {
                    _husband.intervalX = this.settings.size.intervalX + adjust;
                }
                else {
                    // 没有伴侣的, 也要调整右侧间距
                    _people.intervalX = this.settings.size.intervalX + adjust;
                }
            }
            // 得到左边的父或母
            var _parent;
            if (!_people.Father) {
                _parent = _people.Mother;
                if (_people.Mother
                && _people.Mother.Husband
                && _people.Mother.Husband.drawed
                && _people.Mother.Husband.x < _people.Mother.x)
                    _parent = _people.Mother.Husband;
            }
            else if (!_people.Mother)
                _parent = _people.Father;
            else if (_people.Father.x < _people.Mother.x)
                _parent = _people.Father;
            else
                _parent = _people.Mother;
            if (_parent) {
                // 配置节点偏移，迎合父级(可能是父亲或母亲)节点位置
                var maxWidth = Math.max(_parent.MaxChildrenCount, _parent.Husband ? _parent.Husband.MaxChildrenCount : 0) * (this.settings.size.peopleWidth + this.settings.size.intervalX) - this.settings.size.intervalX;
                var parentTotalWidth = _parent.width + _parent.intervalX;
                parentTotalWidth = parentTotalWidth + typeof (_parent.Wife) == "object" ? _parent.Wife.width : 0;
                parentTotalWidth = parentTotalWidth + typeof (_parent.Husband) == "object" ? _parent.Husband.width : 0;
                var shouldBex = _parent.x - (maxWidth - parentTotalWidth) / 2;
                if (_people.x < shouldBex) {
                    _people.x = shouldBex;
                }
            }

            this.drawPeople(_people);
            _people.drawed = true;
            if (_wife) {
                if (!_wife.drawed) {
                    _wife.x = _people.x + _people.width + _people.intervalX;
                    _wife.y = _people.y;
                    _wife.width = this.settings.size.peopleWidth;
                    _wife.height = this.settings.size.peopleHeight;
                    _wife.intervalX = _wife.intervalX || this.settings.size.intervalX;
                    _wife.intervalY = this.settings.size.intervalY;
                    // 夫妻节点中，增大妻子的右调整距离
                    if (_people.Level < this.topLevel && (this.getFlag().ISMyGrandF(_people) || this.getFlag().ID == _people.ID)) {
                        var maxWidth = _people.getRoot().MaxChildrenCount * (this.settings.size.peopleWidth + this.settings.size.intervalX) - this.settings.size.intervalX;
                        var adjust1 = maxWidth + this.settings.defaultPrev.x - _wife.x - _wife.width;
                        _wife.intervalX += adjust1;
                        if (_wife.getRoot().MaxChildrenCount > 2) {
                            maxWidth = _wife.getRoot().MaxChildrenCount * (this.settings.size.peopleWidth + this.settings.size.intervalX) - this.settings.size.intervalX;
                            var adjust2 = (maxWidth - this.settings.size.peopleWidth * 2 - _people.intervalX) / 2;
                            _wife.intervalX += adjust2;
                        }
                    }
                    this.drawPeople(_wife);
                    _wife.drawed = true;
                }
            }

            if (_husband && !(_husband.Father || _husband.Mother) && !_husband.drawed) {
                _husband.x = _people.x + _people.width + _people.intervalX;
                _husband.y = _people.y;
                _husband.width = this.settings.size.peopleWidth;
                _husband.height = this.settings.size.peopleHeight;
                _husband.intervalX = _husband.intervalX || this.settings.size.intervalX;
                _husband.intervalY = this.settings.size.intervalY;
                // 夫妻节点中，增大丈夫的右调整距离
                if (_people.Level < this.topLevel && (this.getFlag().ISMyGrandF(_people) || this.getFlag().ID == _people.ID)) {
                    var maxWidth = _people.getRoot().MaxChildrenCount * (this.settings.size.peopleWidth + this.settings.size.intervalX) - this.settings.size.intervalX;
                    var adjust1 = maxWidth + this.settings.defaultPrev.x - _husband.x - _husband.width;
                    _husband.intervalX += adjust1;
                    maxWidth = _husband.getRoot().MaxChildrenCount * (this.settings.size.peopleWidth + this.settings.size.intervalX) - this.settings.size.intervalX;
                    var adjust2 = (maxWidth - this.settings.size.peopleWidth * 2 - _people.intervalX) / 2;
                    _husband.intervalX += adjust2;
                }
                this.drawPeople(_husband);
                _husband.drawed = true;
            }
        }
    }
    this.Draw = function (peoples) {
        if (!this.DataPrapared) {
            this.throwError("Data has not prapared.");
            return false;
        }
        for (var i = 0; i < peoples.length; i++) {
            for (var j in peoples[i]) {
                var people = peoples[i][j];
                var _people = this.getPeople(people.ID);
                // 计算每一层的第一个节点;
                var _prevPeople = j > 0 ? this.getPeople(peoples[i][j - 1].ID) : _top.jQuery.extend({}, this.settings.defaultPrev,
                    { y: this.settings.defaultPrev.y + (this.settings.size.peopleHeight + this.settings.size.intervalY) * i });
                this.createPeopleRect(people, _prevPeople);
            }
        }
    }

    this.drawRelation = function (points, source, target) {
        if (points.length < 2)
            return false;
        var path = document.createElementNS(SVG, "path");
        path.setAttribute("fill", "none");
        path.setAttribute("stroke", this.settings.color.Line);
        path.setAttribute("stroke-width", "1");
        path.setAttribute("id", "r" + source.ID + "to" + target.ID);
        var pd = "M" + points[0]["x"] + " " + points[0]["y"] + " ";
        for (var i = 1; i < points.length; i++) {
            pd = pd + points[i]["code"] + points[i]["x"] + " " + points[i]["y"] + " ";
        }
        path.setAttribute("d", pd);
        this.relationGroup.appendChild(path);
    }
    this.DrawLine = function () {
        if (!this.DataPrapared) {
            this.throwError("Data has not prapared.");
            return false;
        }
        for (var i = 0; i < this.relations.length; i++) {
            var relation = this.relations[i];
            var source = this.getPeople(relation.SourceID);
            var target = this.getPeople(relation.TargetID);
            var relationType = this.settings.relationMap[relation.Relation];
            if (!source || !target) { continue; }
            if (!document.getElementById("people" + source.ID) || !document.getElementById("people" + target.ID)) {
                continue;
            }
            if (relationType == 0) {
                var path = [];
                path.push({ x: source.x + source.width, y: source.y + source.height / 2 });
                path.push({ code: this.settings.pathCode.lineto, x: target.x, y: target.y + target.height / 2 });
                this.drawRelation(path, source, target);
            }
            if (relationType == 1) {
                var path = [];
                var father;
                var mother;

                if (relation.Relation == "Father") {
                    father = source;
                    mother = target.Mother || father.Wife;
                }
                else if (relation.Relation == "Mother") {
                    mother = source;
                    father = target.Father || mother.Husband;
                }
                // 单亲家庭
                if (!father || !mother) {
                    path.push({ x: source.x + source.width / 2, y: source.y + source.height });
                    path.push({ code: this.settings.pathCode.lineto, x: source.x + source.width / 2, y: source.y + source.height + source.intervalY / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: target.x + target.width / 2, y: source.y + source.height + source.intervalY / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: target.x + target.width / 2, y: target.y });
                    this.drawRelation(path, source, target);
                }
                else if (father.Wife != target.Mother || mother.Husband != target.Father) {
                    var leftparent = father.x < mother.x ? father : mother;
                    path.push({ x: leftparent.x + leftparent.width + leftparent.intervalX / 2, y: leftparent.y + leftparent.height / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: leftparent.x + leftparent.width + leftparent.intervalX / 2, y: leftparent.y + leftparent.height + leftparent.intervalY / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: target.x + target.width / 2, y: source.y + source.height + source.intervalY / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: target.x + target.width / 2, y: target.y });
                    this.drawRelation(path, leftparent, target);

                    // 父母不是夫妻关系
                    var rightparent = father.x > mother.x ? father : mother;
                    path = [];
                    path.push({ x: leftparent.x + leftparent.width, y: leftparent.y + leftparent.height / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: rightparent.x, y: rightparent.y + rightparent.height / 2 });
                    this.drawRelation(path, leftparent, rightparent);
                }
                else {
                    var leftparent = father.x < mother.x ? father : mother;
                    path.push({ x: leftparent.x + leftparent.width + leftparent.intervalX / 2, y: leftparent.y + leftparent.height / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: leftparent.x + leftparent.width + leftparent.intervalX / 2, y: leftparent.y + leftparent.height + leftparent.intervalY / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: target.x + target.width / 2, y: source.y + source.height + source.intervalY / 2 });
                    path.push({ code: this.settings.pathCode.lineto, x: target.x + target.width / 2, y: target.y });
                    this.drawRelation(path, leftparent, target);
                }

            }
        }
    }

    this.createSVGGroupIFNo = function (id, appendToDom) {
        try {
            if (arguments.length == 1) appendToDom = true;
            var g = document.getElementById(id);
            if (!g) {
                g = document.createElementNS(SVG, "g");
                g.setAttribute("id", id);
                if (appendToDom === true) {
                    this.treeDom().appendChild(g);
                }
            }
            return g;
        }
        catch (e) {
            var g = document.createElementNS(SVG, "g");
            g.setAttribute("id", id);
            if (appendToDom === true) {
                this.treeDom().appendChild(g);
            }
            return g;
        }
    }

    this.startDraw = function () {
        this.relationGroup = this.createSVGGroupIFNo("relations");
        this.relationGroup.appendChild(this.createAnimateTran());
        this.tooltipGroup = this.createSVGGroupIFNo("tooltips", true);

        if (!this.DataPrapared) {
            this.PrapareData();
        }
        this.Draw(this.peopleTree, null);
        this.DrawLine();

        this.treeDom().appendChild(this.tooltipGroup);

        this.navSettings = _top.FG.extend({}, _top.FG.navSettings, {
            drag: {
                getX: function () {
                    return self.navSettings.tree.Show.width - self.navSettings.nav.Real.width - 20;
                },
                getY: function () {
                    return self.navSettings.tree.Show.height - self.navSettings.nav.Real.height - 20;
                },
                moveRight: function () {
                    return self.getX() < 0;
                },
                moveLeft: function () {
                    return Math.abs(self.getX()) + self.navSettings.tree.Show.width < self.navSettings.tree.Real.width;
                },
                moveUp: function () {
                    return Math.abs(self.getY()) + self.navSettings.tree.Show.height < self.navSettings.tree.Real.height;
                },
                moveDown: function () {
                    return self.getY() < 0;
                },
                moveY: function (_topY) {
                    if (_topY > 0 && self.navSettings.drag.moveUp() || _topY < 0 && self.navSettings.drag.moveDown()) {
                        var moveY = _topY * self.navSettings.multipe;
                        var nowY = self.getY();

                        self.backgroundNavReal.translateY = parseInt(self.backgroundNavReal.getAttributeNS(null, "transform").split(",")[1].replace(")", ""));
                        self.backgroundNavReal.setAttributeNS(null, "transform", "translate(" + (self.backgroundNavReal.translateY + moveX) + ",0)");

                        self.backgroundNavShow.translateY = parseInt(self.backgroundNavShow.getAttributeNS(null, "transform").split(",")[1].replace(")", ""));
                        self.backgroundNavShow.setAttributeNS(null, "transform", "translate(" + (self.backgroundNavShow.translateY + moveY + _topY) + ",0)");

                        self.setY(nowY - moveY);
                        return true;
                    }
                    return false;
                },
                moveX: function (_left) {
                    if (_left > 0 && self.navSettings.drag.moveLeft() || _left < 0 && self.navSettings.drag.moveRight()) {
                        var moveX = _left * self.navSettings.multipe;
                        var nowX = self.getX();

                        self.backgroundNavReal.translateX = parseInt(self.backgroundNavReal.getAttributeNS(null, "transform").split(",")[0].slice(10));
                        self.backgroundNavReal.setAttributeNS(null, "transform", "translate(" + (self.backgroundNavReal.translateX + moveX) + ",0)");

                        self.backgroundNavShow.translateX = parseInt(self.backgroundNavShow.getAttributeNS(null, "transform").split(",")[0].slice(10));
                        self.backgroundNavShow.setAttributeNS(null, "transform", "translate(" + (self.backgroundNavShow.translateX + moveX + _left) + ",0)");
                        self.setX(nowX - moveX);
                        return true;
                    }
                    return false;
                }
            }
        });
        this.navSettings.tree.Show.width = this.navSettings.tree.Real.width = this.treeDom().width.baseVal.value;
        this.navSettings.tree.Show.height = this.navSettings.tree.Real.height = this.treeDom().height.baseVal.value;
        // 得到tree的真实高宽度
        _top.FG.each(this.peoples, function (index, people) {
            if (people.x + people.width > self.navSettings.tree.Real.width) {
                self.navSettings.tree.Real.width = people.x + people.width + 20;
            }
            if (people.y + people.height > self.navSettings.tree.Real.height) {
                self.navSettings.tree.Real.height = people.y + people.height;
            }
        });
        this.navSettings.multipe = this.navSettings.tree.Show.width / (this.navSettings.nav.Show.width + 2);
        this.navSettings.nav.Show.height = this.navSettings.tree.Show.height / this.navSettings.multipe - 2;

        this.navSettings.nav.Real.width = this.navSettings.tree.Real.width / this.navSettings.multipe - 2;
        this.navSettings.nav.Real.height = this.navSettings.tree.Real.height / this.navSettings.multipe - 2;

        this.backgroundNavReal = this.createSVGGroupIFNo("backgroundNavReal");
        this.backgroundNavReal.setAttributeNS(null, "transform", "translate(0,0)");

        this.backgroundNavShow = this.createSVGGroupIFNo("backgroundNavShow");
        this.backgroundNavShow.setAttributeNS(null, "transform", "translate(0,0)");
        if (this.navSettings.tree.Real.width > this.navSettings.tree.Show.width || this.navSettings.tree.Real.height > this.navSettings.tree.Show.height) {
            var navReal = this.createRect(this.navSettings.drag.getX(), this.navSettings.drag.getY(), this.navSettings.nav.Real.width, this.navSettings.nav.Real.height, {
                fill: this.navSettings.nav.Real.fill,
                stroke: this.navSettings.nav.Real.color,
                id: "navReal",
                stroke__width: 1,
                transform: "translate(0,0)"
            });
            this.backgroundNavReal.appendChild(navReal);
            var navShow = this.createImage("/Images/treenav.png", this.navSettings.drag.getX(), this.navSettings.drag.getY(), this.navSettings.nav.Show.width, this.navSettings.nav.Show.height, {
                transform: "translate(0,0)",
                id: "navShow",
                style: "cursor:move;"
            });
            this.backgroundNavShow.appendChild(navShow);

            if (!this.treeDom().getAttribute("eventListener")) {
                this.backgroundNavShow.addEventListener("mousedown", function (_event) {
                    var evt = _event || window.event;
                    self.navSettings.drag.start = true;
                    self.navSettings.drag.startDragX = evt.clientX || evt.pageX;
                    self.navSettings.drag.startDragY = evt.clientY || evt.pageY;
                }, false);
                this.backgroundNavShow.addEventListener("mousemove", function (_event) {
                    var evt = _event || window.event;
                    if (self.navSettings.drag.start) {
                        var _topY = (evt.clientY || evt.pageY) - self.navSettings.drag.startDragY;
                        var _onceTop = _topY > 0 ? 1 : -1;
                        while (_topY != 0 && self.navSettings.drag.moveY(_onceTop)) {
                            self.navSettings.drag.startDragY = self.navSettings.drag.startDragY + _onceTop;
                            _topY += 0 - _onceTop;
                        }

                        var _left = (evt.clientX || evt.pageX) - self.navSettings.drag.startDragX;
                        var _onceLeft = _left > 0 ? 1 : -1;
                        while (_left != 0 && self.navSettings.drag.moveX(_onceLeft)) {
                            self.navSettings.drag.startDragX = self.navSettings.drag.startDragX + _onceLeft;
                            _left += 0 - _onceLeft
                        }
                    }
                }, false);
                this.backgroundNavShow.addEventListener("mouseout", function (event) {
                    self.navSettings.drag.start = false;
                }, false);
                this.backgroundNavShow.addEventListener("mouseup", function (event) {
                    self.navSettings.drag.start = false;
                }, false);
                this.backgroundNavShow.addEventListener("mouseleave", function (event) {
                    self.navSettings.drag.start = false;
                }, false);
                this.treeDom().setAttribute("eventListener", true);
            }

            // 初始化位置 确保自己(当前登录人)在可视范围内
            if (this.getMySelf().x > this.navSettings.tree.Show.width) {
                var _left = 0;
                var _t = this.getMySelf().x;
                for (var i = 0; i < 10; i++) {
                    _t = _t - this.navSettings.tree.Show.width;
                    if (_t < this.navSettings.tree.Show.width) {
                        _left = _t;
                        break;
                    }
                }
                _left = this.navSettings.tree.Show.width + _left - (this.navSettings.tree.Show.width / 2 - this.settings.size.peopleWidth / 2);
                _left = parseInt((_left / this.navSettings.multipe).toFixed()) - 1;
                var _onceLeft = _left > 0 ? 1 : -1;
                while (_left != 0 && self.navSettings.drag.moveX(_onceLeft)) {
                    self.navSettings.drag.startDragX = self.navSettings.drag.startDragX + _onceLeft;
                    _left += 0 - _onceLeft;
                }
            }
        }
    }

    this.getX = function () {
        return typeof (self.treeDom().currentTranslate.getX) == "function" ? self.treeDom().currentTranslate.getX() : self.treeDom().currentTranslate.x;
    };
    this.setX = function (x) {
        if (typeof (self.treeDom().currentTranslate.setX) == "function") { self.treeDom().currentTranslate.setX(x); }
        else { self.treeDom().currentTranslate.x = x; }
    };
    this.getY = function () {
        return typeof (self.treeDom().currentTranslate.getY) == "function" ? self.treeDom().currentTranslate.getY() : self.treeDom().currentTranslate.y;
    };
    this.setY = function (y) {
        if (typeof (self.treeDom().currentTranslate.setY) == "function") { self.treeDom().currentTranslate.setY(y); }
        else { self.treeDom().currentTranslate.y = y; }
    };

    this.clearGraph = function () {
        //var length = this.treeDom().childNodes.length;
        //for (var i = length - 1; i > 0 ; i--) {
        //    if (this.treeDom().childNodes[i].id != "background") {
        //        svgweb.removeChild(this.treeDom().childNodes[i], this.treeDom().childNodes[i].parentNode);
        //    }
        //}
        svgweb.removeChild(this.treeDom(), this.treeDom().parentNode);
        this.tree = null;
    }
    this.updateData = function () {
        this.peoples = [];
        _top.FG.copyArray(this.peoples, this._peoples);
        this.relations = [];
        _top.FG.copyArray(this.relations, this._relations);
        this.relations = _top.FG.quickSortByOrder(this.relations);

        _top.FG.each(this.peoples, function (index, item) {
            item.fg = self;
        });
        _top.FG.each(this.relations, function (index, item) {
            item.fg = self;
        });
        this.topLevel = 1;
        this.bottomLevel = 0;
        this.DataPrapared = false;
        this.peopleTree = [];
    }
    this.addPeople = function (people) {
        this._peoples.push(people);
    }
    this.addRelation = function (source, relation, target) {
        this._relations.push(new Relation(source, relation, target));
    }
    this.updateData();
}

function doload() {
    if (_top.svgweb && _top.svgLoaded) {
        var fgData = _top.getGraphData();
        if (fgData) {
            var fg = new FamilyGraph({
                domOptions: {
                    container: "container",
                    width: 1200,
                    height: 700,
                    background: '#D2B48C'
                },
                peoples: fgData.peoples,
                relations: fgData.relations,
                flagID: fgData.flagID,
                loginID: fgData.loginID,
                events: {
                    peopleLinkClicked: function (people, event) {
                        _top.peopleLinkClicked(people, event);
                    },
                    peopleAddClicked: function (people, event) {
                        _top.peopleAddClicked(people, event);
                    },
                    peopleEditClicked: function (people, event) {
                        _top.peopleEditClicked(people, event);
                    },
                    peopleOKClicked: function (people, event) {
                        _top.peopleOKClicked(people, event);
                    },
                    peopleDeleteClicked: function (people, event) {
                        _top.peopleDeleteClicked(people, event);
                    },
                    peopleViewTreeClicked: function (people, event) {
                        _top.peopleViewTreeClicked(people, event);
                    }
                }
            });
            if (!fg.DataPrapared) {
                fg.PrapareData();
            }
            fg.startDraw();
        }
        _top.setGraphData(fg);
    }
    else {
        setTimeout(function () { doload(); }, 100);
    }
}