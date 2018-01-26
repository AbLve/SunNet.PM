var FG = {
    inArray: function (element, array) {
        return jQuery.inArray(element, array);
    },
    isArraylike: function (obj) {
        var length = obj.length,
            type = jQuery.type(obj);

        if (jQuery.isWindow(obj)) {
            return false;
        }

        if (obj.nodeType === 1 && length) {
            return true;
        }

        return type === "array" || type !== "function" &&
            (length === 0 ||
            typeof length === "number" && length > 0 && (length - 1) in obj);
    },
    extend: function () {
        var src, copyIsArray, copy, name, options, clone,
		target = arguments[0] || {},
		i = 1,
		length = arguments.length,
		deep = false;

        // Handle a deep copy situation
        if (typeof target === "boolean") {
            deep = target;
            target = arguments[1] || {};
            // skip the boolean and the target
            i = 2;
        }

        // Handle case when target is a string or something (possible in deep copy)
        if (typeof target !== "object" && !jQuery.isFunction(target)) {
            target = {};
        }

        // extend jQuery itself if only one argument is passed
        if (length === i) {
            target = this;
            --i;
        }

        for (; i < length; i++) {
            // Only deal with non-null/undefined values
            if ((options = arguments[i]) != null) {
                // Extend the base object
                for (name in options) {
                    src = target[name];
                    copy = options[name];

                    // Prevent never-ending loop
                    if (target === copy) {
                        continue;
                    }

                    // Recurse if we're merging plain objects or arrays
                    if (deep && copy && (jQuery.isPlainObject(copy) || (copyIsArray = jQuery.isArray(copy)))) {
                        if (copyIsArray) {
                            copyIsArray = false;
                            clone = src && jQuery.isArray(src) ? src : [];

                        } else {
                            clone = src && jQuery.isPlainObject(src) ? src : {};
                        }

                        // Never move original objects, clone them
                        target[name] = jQuery.extend(deep, clone, copy);

                        // Don't bring in undefined values
                    } else if (copy !== undefined) {
                        target[name] = copy;
                    }
                }
            }
        }

        // Return the modified object
        return target;
    },
    quickSortByLevel: function (arr) {
        if (arr.length <= 1) { return arr; }
        var pivotIndex = Math.floor(arr.length / 2);
        var pivot = arr.splice(pivotIndex, 1)[0];
        var left = [];
        var right = [];
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].Level > pivot.Level || (arr[i].Level == pivot.Level && arr[i].Gender > pivot.Gender)) {
                left.push(arr[i]);
            } else {
                right.push(arr[i]);
            }
        }
        return FG.quickSortByLevel(left).concat([pivot], FG.quickSortByLevel(right));
    },
    quickSortByOrder: function (arr) {
        if (arr.length <= 1) { return arr; }
        var pivotIndex = Math.floor(arr.length / 2);
        var pivot = arr.splice(pivotIndex, 1)[0];
        var left = [];
        var right = [];
        for (var i = 0; i < arr.length; i++) {
            if (FG.settings.relationOrder[arr[i].Relation] < FG.settings.relationOrder[pivot.Relation]) {
                left.push(arr[i]);
            } else {
                right.push(arr[i]);
            }
        }
        return FG.quickSortByOrder(left).concat([pivot], FG.quickSortByOrder(right));
    },
    quickSortByParent: function (arr) {
        if (arr.length <= 1) { return arr; }
        var pivotIndex = Math.floor(arr.length / 2);
        var pivot = arr.splice(pivotIndex, 1)[0];
        var left = [];
        var right = [];
        for (var i = 0; i < arr.length; i++) {
            if (!arr[i].getLeftParent() && !pivot.getLeftParent()) {
                left.push(arr[i]);
            }
            else if (!arr[i].getLeftParent() && pivot.getLeftParent()) {
                right.push(arr[i]);
            }
            else if (arr[i].getLeftParent() && !pivot.getLeftParent()) {
                left.push(arr[i]);
            }
            else if (arr[i].getLeftParent().x < pivot.getLeftParent().x) {
                left.push(arr[i]);
            }
            else if (arr[i].getLeftParent().x == pivot.getLeftParent().x) {
                if (arr[i].getRightParent().x <= pivot.getRightParent().x) {
                    left.push(arr[i]);
                }
                else {
                    right.push(arr[i]);
                }
            }
            else {
                right.push(arr[i]);
            }
        }
        return FG.quickSortByParent(left).concat([pivot], FG.quickSortByParent(right));
    },
    createPerson: function (people) {
        var p = new Person();
        this.extend(p, people);
        var birth = new Date(p.Birthday);
        if (birth.getFullYear() <= 1900) {
            p.Birthday = "";
        }
        return p;
    },
    createRelation: function (relation) {
        var r = new Relation(0, 0, 0);
        this.extend(r, relation);
        return r;
    },
    each: function (obj, callback, args) {
        var value,
            i = 0,
            length = obj.length,
            isArray = FG.isArraylike(obj);

        if (args) {
            if (isArray) {
                for (; i < length; i++) {
                    value = callback.apply(obj[i], args);

                    if (value === false) {
                        break;
                    }
                }
            } else {
                for (i in obj) {
                    value = callback.apply(obj[i], args);

                    if (value === false) {
                        break;
                    }
                }
            }

            // A special, fast, case for the most common use of each
        } else {
            if (isArray) {
                for (; i < length; i++) {
                    value = callback.call(obj[i], i, obj[i]);

                    if (value === false) {
                        break;
                    }
                }
            } else {
                for (i in obj) {
                    value = callback.call(obj[i], i, obj[i]);

                    if (value === false) {
                        break;
                    }
                }
            }
        }

        return obj;
    },
    getType: function (o) {
        var _t;
        return ((_t = typeof (o)) == "object" ? o == null && "null" || Object.prototype.toString.call(o).slice(8, -1) : _t).toLowerCase();
    },
    copyArray: function (destination, source) {
        for (var p in source) {
            if (FG.getType(source[p]) == "array" || FG.getType(source[p]) == "object") {
                destination[p] = FG.getType(source[p]) == "array" ? [] : {};
                arguments.callee(destination[p], source[p]);
            }
            else {
                destination[p] = source[p];
            }
        }
        return destination;
    }
}

function Relation(source, relation, target) {
    this.SourceID = source;
    this.Relation = relation;
    this.TargetID = target;
    //Normal Expired
    this.Status = "Normal";

    this.fg = {};
    this.getSourcePeople = function () {
        return this.fg.getPeople(this.SourceID);
    };
    this.getTargetPeople = function () {
        return this.fg.getPeople(this.TargetID);
    };
}
function Person(datas) {
    this.ID = datas && datas["ID"];
    this.Name = datas && datas["Name"];
    this.Gender = datas && datas["Gender"];
    this.Birthday = datas && datas["Birthday"];
    this.Status = datas && datas["Status"];
    this.LittleHeadImage = datas && datas["LittleHeadImage"];

    this.Level = -1000;
    this.MaxChildrenCount = -1;

    this.Father = null;
    this.Mother = null;
    this.Children = [];
    this.Wife = null;
    this.Husband = null;
    this.Ex = [];

    this.fg = {};
    this.AddChild = function (child) {
        if (FG.inArray(child, this.Children) < 0) {
            if (!this.fg.getFlag().ISMyGrandF(child) || this.fg.getFlag().ISMyGrandM(child))
                this.Children.unshift(child);
            else {
                this.Children.push(child);
            }

        }
    }

    this.ISMyGrandF = function (people) {
        if (this.ID == people.ID && this.Gender == this.fg.settings.genderMap.Male)
            return true;
        if (this.Level >= people.Level)
            return false;
        if (!this.Father)
            return false;
        if (this.Father.ID == people.ID)
            return true;
        return this.Father.ISMyGrandF(people);
    }
    this.ISMyGrandM = function (people) {
        if (this.Level >= people.Level)
            return false;
        if (!this.Mother)
            return false;
        if (this.Mother.ID == people.ID)
            return true;
        return this.Mother.ISMyGrandM(people);
    }
    this.UpdateMaxChildrenCount = function () {
        if (this.MaxChildrenCount > 0)
            return this.MaxChildrenCount;
        if (this.Husband && (this.Husband.Father || this.Husband.Mother) && true) {
            // 有丈夫 并且丈夫节点有父母，不计算孩子的数量
            this.MaxChildrenCount = 0;
        }
        else {
            var count1 = 1;
            if (this.Children && this.Children.length) {
                count1 = this.Children.length;
                for (var i = 0; i < this.Children.length; i++) {
                    var child = this.Children[i];
                    if (child.Wife) {
                        count1++;
                    }
                    if (child.Husband && !child.Husband.Father && !child.Husband.Mother) {
                        count1++;
                    }
                    // 祖先的妻子或者本人的妻子在 对方家庭中不占比重
                    if (child.Husband && (child.Husband.Father || child.Husband.Mother) && (
                        (this.fg.getFlag().ISMyGrandF(child.Husband) ||
                        this.fg.getFlag().Wife
                        && this.fg.getFlag().Wife.ID == child.ID) && child.ID != this.fg.flagID
                        )
                        ) {
                        count1--;
                    }
                }
            }
            var count2 = 0;
            if (this.Children && this.Children.length) {
                for (var i = 0; i < this.Children.length; i++) {
                    count2 += this.Children[i].UpdateMaxChildrenCount();
                    count2--;
                }
            }
            // 双亲家庭或者 丈夫的父母没有在关系中存在
            this.MaxChildrenCount = Math.max(count1, count2);
        }
        return this.MaxChildrenCount;
    }
    this.getRoot = function () {
        if (!this.Father && !this.Mother) {
            return this;
        }
        var father = 0;
        if (this.Father) {
            father = this.Father.getRoot();
        }
        var mother = 0;
        if (this.Mother) {
            mother = this.Mother.getRoot();
        }
        return father || mother;
    }

    this.getChildren = function () {
        var cr = this.Children.slice(0);
        if (!cr) { cr = []; }
        if (this.Wife && this.Wife.Children && this.Wife.Children.length) {
            for (var i = 0; i < this.Wife.Children.length; i++) {
                if (FG.inArray(this.Wife.Children[i], cr) < 0) {
                    cr.unshift(this.Wife.Children[i])
                }
            }
        }
        return cr;
    }

    this.getMaxX = function () {
        var max = this;
        if (this.Wife && this.Wife.drawed && this.Wife.x > max.x) {
            max = this.Wife;
        }
        if (this.Husband && this.Husband.drawed && this.Husband.x > max.x) {
            max = this.Husband;
        }
        if (this.Ex && this.Ex.length) {
            for (var i = 0; i < this.Ex.length; i++) {
                if (this.Ex[i].drawed && this.Ex[i].x > max.x) {
                    max = this.Ex[i];
                }
            }
        }
        return max;
    }
    this.getMinX = function () {
        var min = this;
        if (this.Wife && this.Wife.drawed && this.Wife.x < min.x) {
            min = this.Wife;
        }
        if (this.Husband && this.Husband.drawed && this.Husband.x < min.x) {
            min = this.Husband;
        }
        if (this.Ex && this.Ex.length) {
            for (var i = 0; i < this.Ex.length; i++) {
                if (this.Ex[i].drawed && this.Ex[i].x < min.x) {
                    min = this.Ex[i];
                }
            }
        }
        return min;
    }

    this.getLeftParent = function () {
        if (!this.Father)
            return this.Mother;
        if (!this.Mother)
            return this.Father;
        if (this.Father.drawed && this.Mother.drawed) {
            return this.Father.x < this.Mother.x ? this.Father : this.Mother;
        }
        return {};
    }
    this.getRightParent = function () {
        if (!this.Father)
            return this.Mother;
        if (!this.Mother)
            return this.Father;
        if (this.Father.drawed && this.Mother.drawed) {
            return this.Father.x > this.Mother.x ? this.Father : this.Mother;
        }
        return {};
    }

    this.getExHeight = function () {
        if (typeof this.exIndex == "undefined") return 0;
        var height = 0 - (this.exIndex + 1) * FG.settings.objects.People.ExHeight;
        return height;
    }
}
FG.settings = {
    debug: true,
    genderMap: {
        Male: 1,
        Female: 0,
        None: -1,
        getByValue: function (value) {
            for (var i in this) {
                if (typeof i != "function" && this[i] == value)
                { return i; }
            }
            return "";
        }
    },
    relationMap: {
        Husband: 0,
        Wife: 0,
        Father: 1,
        Mother: 1,
        getByValue: function (value) {
            for (var i in this) {
                if (typeof i != "function" && this[i] == value)
                { return i; }
            }
            return "";
        }
    },
    statusMap: {
        None: -100,
        Normal: 1,
        Uninitialized: 2
    },
    relationOrder: {
        Husband: 1,
        Wife: 2,
        Father: 3,
        Mother: 4
    },
    color: {
        Male: "#73c5ff",
        Female: "#8bcb31",
        None: "#7cbc13",
        Line: "#555555",
        Self: "#839E00",
        Requesting: "#F7A430"
    },
    objects: {
        Svg: {
            width: 1100,
            height: 700,
            background: '#D2B48C'
        },
        Logo: {
            Top: 15,
            Left: 15,
            Width: 50,
            Height: 50,
            Border: {
                Width: 1,
                Color: "#ffffff"
            }
        },
        Name: {
            Top: 18,
            Left: 75,
            Width: 110,
            Height: 20,
            Color: "#ffffff"
        },
        BirthDay: {
            Top: 38,
            Left: 75,
            Width: 110,
            Height: 20,
            Color: "#c2fffd"
        },
        Button: {
            Top: 58,
            Left: 125,
            Width: 16,
            Height: 16,
            Space: 10
        },
        Tooltip: {
            Top: -25,
            Left: 5,
            Width: 140,
            Height: 25,
            Fill: "#222222",
            FontSize: 12,
            Border: "#000000",
            Background: "#F2F2F3"
        },
        People: {
            rx: 10,
            ry: 10,
            Width: 210,
            Height: 80,
            intervalX: 50,
            intervalY: 40,
            ExHeight: 5
        }
    },
    pathCode: {
        moveto: " M",
        lineto: " L",
        horizontal_lineto: " H",
        vertical_lineto: " V",
        curveto: " C",
        smooth_curveto: " S",
        quadratic_Belzier_curve: " Q",
        smooth_quadratic_Belzier_curveto: " T",
        elliptical_Arc: " A",
        closepath: " Z"
    },
    defaultPrev: {
        x: 20,
        y: 20,
        width: 0,
        height: 0,
        intervalX: 0,
        intervalY: 0
    },
    navigation: function (fgObj) {
        var self = this;
        this.fg = fgObj;
        this.multipe = 0;
        this.tree = {
            Real: {
                width: 0,
                height: 0
            },
            Show: {
                width: 0,
                height: 0
            }
        };
        this.nav = {
            Real: { width: 0, height: 0, color: "#dddddd", fill: "none" },
            Show: { width: 108, height: 0, color: "Blue", fill: "#DDE1E3" }
        };
        this.drag = {
            start: false,
            startDragX: 0,
            startDragY: 0,
            getX: function () {
                return self.tree.Show.width - self.nav.Real.width - 20;
            },
            getY: function () {
                return self.tree.Show.height - self.nav.Real.height - 20;
            },
            moveRight: function () {
                return self.fg.getX() < 0;
            },
            moveLeft: function () {
                return Math.abs(self.fg.getX()) + self.tree.Show.width < self.tree.Real.width;
            },
            moveUp: function () {
                return Math.abs(self.fg.getY()) + self.tree.Show.height < self.tree.Real.height;
            },
            moveDown: function () {
                return self.fg.getY() < 0;
            },
            moveY: function (_topY) {
                if (_topY > 0 && self.drag.moveUp() || _topY < 0 && self.drag.moveDown()) {
                    var moveY = _topY * self.multipe;
                    var nowY = self.fg.getY();
                    var tf = new Transform(self.fg.backgroundNavReal.getAttributeNS(null, "transform"));
                    tf.y = tf.y + moveY;
                    self.fg.backgroundNavReal.setAttributeNS(null, "transform", tf.toString());

                    var tf2 = new Transform(self.fg.backgroundNavShow.getAttributeNS(null, "transform"));
                    tf2.y = tf2.y + moveY + _topY;
                    self.fg.backgroundNavShow.setAttributeNS(null, "transform", tf2.toString());

                    self.fg.setY(nowY - moveY);
                    return true;
                }
                return false;
            },
            moveX: function (_left) {
                if (_left > 0 && self.drag.moveLeft() || _left < 0 && self.drag.moveRight()) {
                    var moveX = _left * self.multipe;
                    var nowX = self.fg.getX();

                    var tf = new Transform(self.fg.backgroundNavReal.getAttributeNS(null, "transform"));
                    tf.x = tf.x + moveX;
                    self.fg.backgroundNavReal.setAttributeNS(null, "transform", tf.toString());

                    var tf2 = new Transform(self.fg.backgroundNavShow.getAttributeNS(null, "transform"));
                    tf2.x = tf2.x + moveX + _left;
                    self.fg.backgroundNavShow.setAttributeNS(null, "transform", tf2.toString());

                    self.fg.setX(nowX - moveX);
                    return true;
                }
                return false;
            }
        }
    }
}
function Transform(transform) {
    var items = transform.split(",");
    this.x = parseFloat(items[0].replace("translate(", ""));
    this.y = parseFloat(items[1].replace(")", ""));
    this.toString = function () {
        return "translate(" + this.x + "," + this.y + ")";
    }
}
