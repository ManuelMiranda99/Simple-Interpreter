import { Type } from "../Abstract/Return";
import { Symbol } from "./Symbol";

export class Environment {
  public variables: Map<string, Symbol>;
  public functions;

  constructor(public previous: Environment | null) {
    this.variables = new Map();
    this.functions = {};
  }

  public saveVariable(id: string, value: any, type: Type) {
    this.variables.set(id, new Symbol(value, id, type, 0, 0));
  }

  public getVariable(id: string): Symbol | null {
    let variable = this.variables.get(id);
    if (variable) return variable;
    if (this.previous) return this.previous.getVariable(id);
    return null;
  }
}
