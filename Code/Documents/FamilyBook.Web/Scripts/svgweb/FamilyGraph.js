if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (elt /*, from*/) {
        var len = this.length >>> 0;

        var from = Number(arguments[1]) || 0;
        from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
        if (from < 0)
            from += len;

        for (; from < len; from++) {
            if (from in this &&
          this[from] === elt)
                return from;
        }
        return -1;
    };
};
var FG = {
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
            if (FG.defaultSettings.relationOrder[arr[i].relation] < FG.defaultSettings.relationOrder[pivot.relation]) {
                left.push(arr[i]);
            } else {
                right.push(arr[i]);
            }
        }
        return FG.quickSortByOrder(left).concat([pivot], FG.quickSortByOrder(right));
    },
    createPerson: function (id, name, gender, birthday, status, headImage, isVirtualAccount, isOK, level, maxChildrenCount) {
        return new Person({
            ID: id,
            Name: name,
            Gender: gender,
            Birthday: birthday,
            Status: status,
            LittleHeadImage: headImage,
            Level: level,
            virtualAccount: isVirtualAccount,
            IsOK: isOK,
            MaxChildrenCount: maxChildrenCount
        });
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
    },
    defaultSettings: {
        Gender: {
            Male: 1,
            Female: 0,
            None: -1
        },
        relationMap: {
            Husband: 0,
            Wife: 0,
            Father: 1,
            Mother: 1
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
            Line: "#555555"
        },
        attribute: {
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
            Action: {
                Top: 58,
                Left: 125,
                Width: 16,
                Height: 16,
                Space: 10
            }
        },
        size: {
            rx: 10,
            ry: 10,

            peopleWidth: 210,
            peopleHeight: 80,
            intervalX: 80,
            intervalY: 50
        },
        pathCode: {
            moveto: "M",
            lineto: "L",
            horizontal_lineto: "H",
            vertical_lineto: "V",
            curveto: "C",
            smooth_curveto: "S",
            quadratic_Belzier_curve: "Q",
            smooth_quadratic_Belzier_curveto: "T",
            elliptical_Arc: "A",
            closepath: "Z"
        },
        defaultPrev: {
            x: 20,
            y: 20,
            width: 0,
            height: 0,
            intervalX: 0,
            intervalY: 0
        }
    },
    navSettings: {
        tree: {
            Real: {
                width: 0,
                height: 0
            },
            Show: {
                width: 0,
                height: 0
            }
        },
        nav: {
            Real: { width: 0, height: 0, color: "#dddddd", fill: "none" },
            Show: { width: 108, height: 0, color: "Blue", fill: "#DDE1E3" }
        },
        drag: {
            start: false,
            startDragX: 0,
            startDragY: 0
        }
    }
}


function Relation(source, relation, target) {
    this.source = source;
    this.relation = relation;
    this.target = target;
    this.fg = {};
    this.getSourcePeople = function () {
        return this.fg.getPeople(this.source);
    };
    this.getTargetPeople = function () {
        return this.fg.getPeople(this.target);
    };
}
function Person(datas) {
    this.ID = datas["ID"];
    this.Name = datas["Name"];
    this.Gender = datas["Gender"];
    this.Birthday = datas["Birthday"];
    this.Status = datas["Status"];
    this.LittleHeadImage = datas["LittleHeadImage"];
    this.IsOK = datas["IsOK"];

    this.virtualAccount = datas["virtualAccount"];
    this.Level = 0;
    this.MaxChildrenCount = 0;

    this.Father = null;
    this.Mother = null;
    this.Children = [];
    this.Wife = null;
    this.Husband = null;

    this.fg = {};
    this.AddChild = function (child) {
        if (this.Children.indexOf(child) < 0) {
            if (!this.fg.getFlag().ISMyGrandF(child) || this.fg.getFlag().ISMyGrandM(child))
                this.Children.unshift(child);
            else {
                this.Children.push(child);
            }

        }
    }

    this.ISMyGrandF = function (people) {
        if (this.ID == people.ID)
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
        if (this.Husband && this.Husband.Father && this.Husband.Mother && true) {
            // 双亲家庭的妈妈，不计算孩子的数量
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
                    if (this.Husband && !this.Husband.Father && !this.Husband.Mother) {
                        count1++;
                    }
                    // 祖先的妻子或者本人的妻子在 对方家庭中不占比重
                    if (child.Husband && (
                        this.fg.getFlag().ISMyGrandF(child.Husband) ||
                        this.fg.getFlag().Wife && this.fg.getFlag().Wife.ID == child.ID)) {
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
            // 双亲家庭或者 老公父母没有在关系中存在
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
}

